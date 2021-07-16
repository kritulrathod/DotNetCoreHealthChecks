using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreHealthChecks.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DotNetCoreHealthChecks
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllers();
      _ = services.AddHealthChecks()
        .AddCheck<RandomResponseTimeChecker>("API Custom Health", null, new[] { "API" })
        //TODO : Check the hints  here - https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks#UI-Storage-Providers
        .AddCheck<RandomResponseTimeChecker>("DATABASE Custom Health", null, new[] { "DATABASE" })
        .AddCheck("API Health", () => HealthCheckResult.Healthy("DotNetCoreHealthChecks Works"));
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetCoreHealthChecks", Version = "v1" });
      });

      services.AddSingleton<RandomResponseTimeChecker>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetCoreHealthChecks v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
      });
    }
  }
}
