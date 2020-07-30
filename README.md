# netcore-boilerplate

Boilerplate of API in `.NET Core 3.1`

Boilerplate is a piece of code that helps you to quickly kick-off a project or start writing your source code. It is kind of a template - instead
of starting an empty project and adding the same snippets each time, you can use the boilerplate that already contains such code.
The Project is configured to run unit tests while pushing, to setup your local for this, run
`git config core.hooksPath git-hooks/`

## Source code contains

1. [Swagger](https://swagger.io/) + [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)
1. HealthChecks
    Application has 2 types of Health checks, Liveness and Readiness. These are exposed via 3 endpoints:
    Visit http://localhost:5000/health to run ALL health checks
    Visit http://localhost:5000/health/live to run Liveness only health checks
    Visit http://localhost:5000/health/ready to run Readiness only health checks

Sample response from health checks
```json
{
  "status": "Healthy",
  "results": {
    "LivenessHealthCheck": {
      "status": "Healthy",
      "description": "Live and Healthy!",
      "data": {}
    },
    "ReadinessCredentialsHealthCheck": {
      "status": "Healthy",
      "description": "Ready and Healthy!",
      "data": {}
    },
    "ReadinessPingHealthCheck": {
      "status": "Healthy",
      "description": "SAMPLE_VARIABLE endpoint is up!",
      "data": {}
    }
  }
}
```

1. Tests
    * Unit tests
        * [AutoFixture](https://github.com/AutoFixture/AutoFixture)
        * [FluentAssertions]
        * [Moq](https://github.com/moq/moq4)
        * [Moq.AutoMock](https://github.com/moq/Moq.AutoMocker)
        * xUnit
1. Code quality
    * [EditorConfig](https://editorconfig.org/) ([.editorconfig](.editorconfig))
    * Analizers ([Microsoft.CodeAnalysis.Analyzers](https://github.com/dotnet/roslyn-analyzers), [Microsoft.AspNetCore.Mvc.Api.Analyzers](https://github.com/aspnet/AspNetCore/tree/master/src/Analyzers))
    * [Rules](Coop.Sample.ruleset)
    * Code coverage
        * [Coverlet](https://github.com/tonerdo/coverlet)
        * [Codecov](https://codecov.io/)
1. Docker
    * [Dockerfile](dockerfile)
    * [Docker-compose](docker-compose.yml)
        * `netcore-boilerplate:local`
1. [Serilog](https://serilog.net/)
    * Sink: [Async](https://github.com/serilog/serilog-sinks-async)

## Architecture

### Api

[Coop.Sample.Api](src/Coop.Sample.Api)

* Simple Startup class - [Startup.cs](src/Coop.Sample.Api/Startup.cs)
  * MvcCore
  * Swagger (Swashbuckle)
  * HttpClient
  * HealthChecks
* Filters
  * Action filter to validate `ModelState` - [ValidateModelStateFilter.cs](src/Coop.Sample.Api/Infrastructure/Filters/ValidateModelStateFilter.cs)
  * Global exception filter - [HttpGlobalExceptionFilter.cs](src/Coop.Sample.Api/Infrastructure/Filters/HttpGlobalExceptionFilter.cs)
* Configurations
  * `Serilog` configuration place - [SerilogConfigurator.cs](src/Coop.Sample.Api/Infrastructure/Configurations/SerilogConfigurator.cs)
  * `Swagger` configuration place - [SwaggerConfigurator.cs](src/Coop.Sample.Api/Infrastructure/Configurations/SwaggerConfigurator.cs)
* Simple exemplary API controllers - [EmployeesController.cs](src/Coop.Sample.Api/Controllers/EmployeesController.cs), [CarsController.cs](src/Coop.Sample.Api/Controllers/CarsController.cs)

### Core

[Coop.Sample.Core](src/Coop.Sample.Core)

* Exemplary repository - [EmployeeRepository.cs](src/Coop.Sample.Core/Repositories/EmployeeRepository.cs)
* Exemplary service - [CarService.cs](src/Coop.Sample.Core/Services/CarService.cs)

## Tests

### Unit tests

[Coop.Sample.Api.UnitTests](test/Coop.Sample.Api.UnitTests)
[Coop.Sample.Core.UnitTests](test/Coop.Sample.Core.UnitTests)

## Checking test coverage
In the ``test/Coop.Sample.Api.UnitTests`` folder,
step 1 : run ``dotnet test /p:CollectCoverage=true`` which will create ``coverage.json``
step 2 : run ``dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover`` which will create ``coverage.opencover.xml``
dev can filter result to remove extra Assembly using ``/p:Exclude=[Assembly-Filter]Type-Filter`` Ex. ``/p:Exclude=\"[Coop.Sample.Api.*]*,[*]*\", /p:Include=\"[Coop.Sample.Api.UnitTests]*,[*]Controller*\"``
step 3 : install ReportGenerator tool to do that run ``dotnet tool install -g dotnet-reportgenerator-globaltool``
step 4 : Generate Report run ``reportgenerator "-reports:coverage.opencover.xml" "-targetdir:report"`` which will create folder ``test/Coop.Sample.Api.UnitTests/report``
step 5 : open index.html under ``test/Coop.Sample.Api.UnitTests/report`` on browser to see report.

For more information for test coverage visit https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/MSBuildIntegration.md#source-files

## How to adapt to your project

Generally it is totally up to you! But in case you do not have any plan, You can follow below simple steps:

1. Download/clone/fork repository
1. Remove components and/or classes that you do not need to
1. Rename files (e.g. sln, csproj, ruleset), folders, namespaces etc.
1. Give us a star!

## Build the solution

Just execute `dotnet build` in the root directory, it takes `Coop.Sample.sln` and build everything.

## Start the application

### Standalone

Then the application (API) can be started by `dotnet run` command executed in the `src/Coop.Sample.Api` directory.
By default it will be available under http://localhost:5000/, but keep in mind that documentation is available under
http://localhost:5000/api-docs/.

### Docker (recommended)

In order to run the project locally, run
    `docker-compose up` to run as process in terminal OR
    `docker-compose up -d` to run in background OR
    `docker-compose up --build` to force new image build and run (using any of the above options) in repo's root directory. This should build (if required) and start the application on port 80.

Then visit: http://localhost/api-docs/

#### Overriding docker files
In order to run docker locally with custom settings,

Copy docker-compose.override-example.yml to docker-compose.override.yml.
Add credentials via environment variable section. You can also add new or update existing settings if required.
Now run `docker-compose up` and visit http://localhost:5000/api-docs/

_The .override file overrides the base docker-compose.yml file automatically when present. This file is part of .gitignore and should never be pushed as it's your local setup file and could also have credentials._


## Run unit tests

Run `dotnet test` command in the root directory, it will look for test projects in `Coop.Sample.sln` and run them.
