using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Wrap;
using Potter.Characters.IntegrationService.PotterApi.Configurations;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Potter.Characters.Api.Configurations
{
    public static class PotterApiConfiguration
    {
        public static void AddPotterApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Carrega as configurações da API de integração
            services.Configure<PotterApiConfig>(configuration.GetSection(nameof(PotterApiConfig)));
            services.AddSingleton<IPotterApiConfig>(x => x.GetRequiredService<IOptions<PotterApiConfig>>().Value);

            IAsyncPolicy<HttpResponseMessage> retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    3, 
                    retryAttempt => TimeSpan.FromSeconds(retryAttempt),
                    onRetry: (message, retryCount) =>
                    {
                        Console.Out.WriteLine($"Content: {message.Result.Content.ReadAsStringAsync()}");
                        Console.Out.WriteLine($"ReasonPhrase: {message.Result.ReasonPhrase}");
                        Console.Out.WriteLine($"RetryCount: {retryCount}");
                    });

            IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(
                    3,
                    TimeSpan.FromSeconds(30),
                    onBreak: (message, timespan) => {
                        Console.Out.WriteLine($"Content: {message.Result.Content.ReadAsStringAsync()}");
                        Console.Out.WriteLine($"ReasonPhrase: {message.Result.ReasonPhrase}");
                    }, onReset: () => {
                        Console.Out.WriteLine("Reset");
                    });

            IAsyncPolicy<HttpResponseMessage> policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

            var baseUrl = configuration.GetSection("PotterApiConfig")?.GetSection("BaseUrl").Value?.TrimEnd('/');
            services.AddHttpClient<IPotterApiCharacterService, PotterApiCharacterService>
                (x => x.BaseAddress = new Uri(baseUrl))
                .AddPolicyHandler(policyWrap);

            services.AddHttpClient<IPotterApiHouseService, PotterApiHouseService>
                (x => x.BaseAddress = new Uri(baseUrl))
                .AddPolicyHandler(policyWrap);
        }
    }
}
