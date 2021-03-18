using System;
using Klinkby.Clam;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Klinkby.ClamREST
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Klinkby.ClamREST", Version = "v1" });
            });
            services.AddSingleton<IAppConfiguration>(_ => AppConfiguration.FromConfiguration(Configuration));
            services.AddTransient(ClamClientFactory);
        }

        internal static IClamClient ClamClientFactory(IServiceProvider svcs)
        {
            var cfg = svcs.GetService<IAppConfiguration>();
            var logger = svcs.GetService<ILogger<IClamClient>>();
            return 0 == cfg.IPAdresses.Length ?
                new ClamClient(cfg.Host, cfg.Port, logger) :
                new ClamClient(cfg.IPAdresses, cfg.Port, logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Klinkby.ClamREST v1"));
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
