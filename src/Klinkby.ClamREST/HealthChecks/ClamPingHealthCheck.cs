using Klinkby.Clam;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Klinkby.ClamREST.HealthChecks
{
    public class ClamPingHealthCheck : IHealthCheck
    {
        private readonly Func<IClamClient> clamClientFactory;

        public ClamPingHealthCheck(Func<IClamClient> clamClientFactory)
        {
            this.clamClientFactory = clamClientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                using (var clamClient = clamClientFactory())
                {
                    await clamClient.PingAsync().ConfigureAwait(false);
                }
                return sw.ElapsedMilliseconds > 100 ? HealthCheckResult.Degraded() : HealthCheckResult.Healthy();
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
