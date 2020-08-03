using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Potter.Characters.IntegrationService.PotterApi.Configurations;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Service;
using System;
using System.Net.Http;

namespace Potter.Characters.Api.Configurations
{
    public static class PotterApiConfiguration
    {
        public static void AddPotterApiConfiguration(this IServiceCollection services, IConfiguration configuration/*, ILogger<Startup> logger*/)
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
                        //logger.LogWarning($"Retry polyci -> Count: {retryCount} Message: {message.Result.Content.ReadAsStringAsync()}");
                    });

            IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(
                    3,
                    TimeSpan.FromSeconds(30),
                    onBreak: (message, timespan) =>
                    {
                        //logger.LogWarning($"CircuitBreaker polyci (BREAK) -> Timespan: {timespan} Message: {message.Result.Content.ReadAsStringAsync()}");
                    },
                    onReset: () =>
                    {
                        //logger.LogWarning($"CircuitBreaker polyci (RESET) ");
                    },
                    onHalfOpen: () => 
                    {
                        //logger.LogWarning($"CircuitBreaker polyci (HALF OPEN)");
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
