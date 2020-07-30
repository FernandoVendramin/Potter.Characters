using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Potter.Characters.Api.Configurations;
using Potter.Characters.Api.Controllers;
using Potter.Characters.Application.PotterApi.Configurations;
using Potter.Characters.Application.PotterApi.Interfaces;
using Potter.Characters.Application.PotterApi.Services;
using System;
using System.Net.Http;

namespace Potter.Characters.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Carrega as configurações da API de integração
            services.Configure<PotterApiConfig>(Configuration.GetSection(nameof(PotterApiConfig)));
            services.AddSingleton<IPotterApiConfig>(x => x.GetRequiredService<IOptions<PotterApiConfig>>().Value);

            // implementar LOGs !!!!!!!!!
            var retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .RetryAsync(2, onRetry: (message, retryCount) =>
                {
                    Console.Out.WriteLine($"Content: {message.Result.Content.ReadAsStringAsync()}" );
                    Console.Out.WriteLine($"ReasonPhrase: {message.Result.ReasonPhrase}");
                    Console.Out.WriteLine($"RetryCount: {retryCount}");
                });

            services.AddHttpClient<IPotterApiCharacterService, PotterApiCharacterService>
                (x => x.BaseAddress = new Uri(Configuration["PotterApiConfig:BaseUrl"]?.TrimEnd('/')))
                .AddPolicyHandler(retryPolicy);

            // Configura DataContext
            services.AddDataContext(Configuration);
            // Configura a IOC
            services.AddDependencyInjections();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // AutoMigration do banco
            app.AddDataContextConfigureAutoMigrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(ep => 
            {
                ep.MapControllers();
            });
        }
    }
}
