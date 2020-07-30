using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Coop.Sample.Api.HealthChecks
{
    public class ReadinessPingHealthCheck : IHealthCheck
    {
        private readonly IConfiguration Configuration;
        public ReadinessPingHealthCheck(IConfiguration configuration) {
            Configuration = configuration;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            string faurl = Configuration["AppSettings:SAMPLE_VARIABLE"];
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(faurl);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"SAMPLE_VARIABLE responding with: {response.StatusCode}");
                    }
                }
                catch (Exception e)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy(e.Message));
                }
            }
            return await Task.FromResult(HealthCheckResult.Healthy("SAMPLE_VARIABLE endpoint is up!"));
        }
    }
}
