using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
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

            var retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .RetryAsync(2, onRetry: (message, retryCount) =>
                {
                    Console.Out.WriteLine($"Content: {message.Result.Content.ReadAsStringAsync()}");
                    Console.Out.WriteLine($"ReasonPhrase: {message.Result.ReasonPhrase}");
                    Console.Out.WriteLine($"RetryCount: {retryCount}");
                });

            services.AddHttpClient<IPotterApiCharacterService, PotterApiCharacterService>
                (x => x.BaseAddress = new Uri(configuration["PotterApiConfig:BaseUrl"]?.TrimEnd('/')))
                .AddPolicyHandler(retryPolicy);
        }
    }
}
