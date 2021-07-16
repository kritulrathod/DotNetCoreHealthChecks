using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreHealthChecks.HealthChecks
{
  public class RandomResponseTimeChecker : IHealthCheck
  {
    private static Random rndm = new Random();

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      int responseTime = rndm.Next(1, 300);
      if (responseTime < 100)
        return Task.FromResult(HealthCheckResult.Healthy($"Healthy. {responseTime} ms."));
      else if (responseTime >= 100 && responseTime < 200)
        return Task.FromResult(HealthCheckResult.Degraded($"Degraded. {responseTime} ms."));
      else
        return Task.FromResult(HealthCheckResult.Unhealthy($"Unhealthy. {responseTime} ms."));
    }
  }
}
