using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Potter.Characters.IntegrationService.PotterApi.Service
{
    public class PotterApiCharacterService : IPotterApiCharacterService
    {
        private readonly IPotterApiConfig _potterApiConfig;
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        public PotterApiCharacterService(IPotterApiConfig potterApiConfig, HttpClient httpClient, IDistributedCache cache)
        {
            _potterApiConfig = potterApiConfig;
            _httpClient = httpClient;
            _cache = cache;
        }
        public async Task<List<PotterApiCharacter>> GetByNameAsync(string name)
        {
            var potterApiCharacters = new List<PotterApiCharacter>();

            string potterApiCache = _cache.GetString("PotterApiCharacters");

            if (potterApiCache == null)
            {
                DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                potterApiCharacters = await _httpClient.GetFromJsonAsync<List<PotterApiCharacter>>(
                    $"v1/characters?key={_potterApiConfig.Key}");

                potterApiCache = JsonConvert.SerializeObject(potterApiCharacters);

                _cache.SetString("PotterApiCharacters", potterApiCache, opcoesCache);
            }
            else
            {
                potterApiCharacters = JsonConvert
                    .DeserializeObject<List<PotterApiCharacter>>(potterApiCache);
            }

            potterApiCharacters = potterApiCharacters.Where(x => x.name == name).ToList();

            return potterApiCharacters;
        }
    }
}
