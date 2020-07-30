using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Coop.Sample.Api.Infrastructure.Registrations
{
    public static class SwaggerRegistration
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            string secretKey = configuration.GetValue<string>("ApiKey:SecretKey");

            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sample Api",
                    Version = "v1",
                    Description = "Sample Api Boilerplate",
                    Contact = new OpenApiContact
                    {
                        Name = "AX Integration team",
                        Url = new Uri("https://confluence.cooperators.ca/display/AXP/Integration+Team"),
                    }
                });

                swaggerOptions.OrderActionsBy(x => x.RelativePath);
                swaggerOptions.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Coop.Sample.Api.xml"));
            });
        }
    }
}
