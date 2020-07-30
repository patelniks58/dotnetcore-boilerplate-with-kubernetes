using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;

namespace Coop.Sample.Api.HealthChecks
{
    // Validate that all credentials are set via environment variables
    public class ReadinessCredentialsHealthCheck : IHealthCheck
    {
        private readonly IConfiguration Configuration;
        public ReadinessCredentialsHealthCheck(IConfiguration configuration) {
            Configuration = configuration;
        }
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            string url = Configuration["AppSettings:SAMPLE_VARIABLE"];

            string response = null;

            response = string.IsNullOrEmpty(url) ? response + "AppSettings:SAMPLE_VARIABLE, " : response;

            if (response == null)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("Ready and Healthy!"));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy($"No value found for: {response}. Set via appsettings.Development.json file locally or 'Environment Variables' in Docker environment (AppSettings__SAMPLE_VARIABLE: 'value')"));
        }
    }
}
