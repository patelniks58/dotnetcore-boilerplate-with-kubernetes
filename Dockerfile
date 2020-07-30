FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /App
COPY . .

# Add SSL Cert
RUN apk update && apk add ca-certificates && rm -rf /var/cache/apk/*
COPY ./Build/CoopRootCA-G2A.crt /usr/local/share/ca-certificates/CoopRootCA-G2A.crt
RUN update-ca-certificates

# For the unit tests Build and Run
WORKDIR /App
RUN dotnet restore

RUN dotnet build -c Release -o /App/build

# Run the unit tests
RUN dotnet test --filter TestType=Unit

# Publish
WORKDIR /App/src/Coop.Sample.Api
RUN dotnet publish -c Release -o /App/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS publish
ENV PORT 80
EXPOSE 80
# Update and install curl for health checks
RUN apk update && apk upgrade && apk add curl

# Set correct timezone
RUN apk add --no-cache tzdata
ENV TZ America/Toronto

WORKDIR /App
COPY --from=build /App/publish .
ENTRYPOINT ["dotnet", "Coop.Sample.Api.dll"]

HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 CMD curl --fail http://localhost/health/live || exit 1
