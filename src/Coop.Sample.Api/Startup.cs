using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Mvc;
using Coop.Sample.Api.Infrastructure.Filters;
using Microsoft.Extensions.Hosting;
using Coop.Sample.Api.Infrastructure.Registrations;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Coop.Sample.Api.HealthChecks;
using Serilog;
using Coop.Sample.Core.Services;
using Coop.Sample.Core.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.IO;
using System.Text;

namespace Coop.Sample.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                    options.Filters.Add<ValidateModelStateFilter>();
                })
                .AddApiExplorer()
                .AddDataAnnotations()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSingleton<ICarService, CarService>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

            services.AddHttpContextAccessor();

            services.AddSwagger(_configuration);

            services.AddHealthChecks()
                // Tag each with `ready` or `live` so it's run via correct endpoint
                .AddCheck<LivenessHealthCheck>(
                    "LivenessHealthCheck",
                    failureStatus: null, // HealthStatus.Unhealthy (default)
                    tags: new[] { "live" })

                .AddCheck<ReadinessCredentialsHealthCheck>(
                    "ReadinessCredentialsHealthCheck",
                    failureStatus: null,
                    tags: new[] { "ready" })

                .AddCheck<ReadinessPingHealthCheck>(
                    "ReadinessPingHealthCheck",
                    failureStatus: null,
                    tags: new[] { "ready" });
        }


        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // This needs to be before routing
            app.UseSerilogRequestLogging(c =>
            {
                c.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Setup Health Checks endpoints (3)
                // 1. All checks
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = CustomHealthCheckResponse
                });

                // 2. Only readiness checks
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = CustomHealthCheckResponse
                });

                // 3. Only liveness checks
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("live"),
                    ResponseWriter = CustomHealthCheckResponse
                });
            });

            // Use OAS 2.0 instead of 3 to deploy properly using APIC (IBM cloud)
            app.UseSwagger(c => { c.SerializeAsV2 = true; });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = _configuration["AppSettings:DocsUrl"];
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api V1");
                c.DocExpansion(DocExpansion.None);
            });
        }

        // Custom Health Check 'ResponseWriter' to output as desired JSON object
        // This approach returns a json with overall "status" and list of each
        // Health Check with detailed information instead of just a string.
        private static Task CustomHealthCheckResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    writer.WriteString("status", result.Status.ToString());
                    writer.WriteStartObject("results");
                    foreach (var entry in result.Entries)
                    {
                        writer.WriteStartObject(entry.Key);
                        writer.WriteString("status", entry.Value.Status.ToString());
                        writer.WriteString("description", entry.Value.Description);
                        writer.WriteStartObject("data");
                        foreach (var item in entry.Value.Data)
                        {
                            writer.WritePropertyName(item.Key);
                            JsonSerializer.Serialize(
                                writer, item.Value, item.Value?.GetType() ??
                                typeof(object));
                        }
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return context.Response.WriteAsync(json);
            }
        }
    }
}
