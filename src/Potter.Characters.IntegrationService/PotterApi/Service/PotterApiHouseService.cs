using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Potter.Characters.IntegrationService.PotterApi.Service
{
    public class PotterApiHouseService : IPotterApiHouseService
    {
        private readonly IPotterApiConfig _potterApiConfig;
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        public PotterApiHouseService(IPotterApiConfig potterApiConfig, HttpClient httpClient, IDistributedCache cache)
        {
            _potterApiConfig = potterApiConfig;
            _httpClient = httpClient;
            _cache = cache;
        }
        public async Task<List<PotterApiHouse>> GetAllAsync()
        {
            var potterApiHouses = new List<PotterApiHouse>();

            string potterApiCache = _cache.GetString("PotterApiHouses");

            if (potterApiCache == null)
            {
                DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                potterApiHouses = await _httpClient.GetFromJsonAsync<List<PotterApiHouse>>
                    ($"v1/houses?key={_potterApiConfig.Key}");

                potterApiCache = JsonConvert.SerializeObject(potterApiHouses);

                _cache.SetString("PotterApiHouses", potterApiCache, opcoesCache);
            }
            else
            {
                potterApiHouses = JsonConvert
                    .DeserializeObject<List<PotterApiHouse>>(potterApiCache);
            }

            return potterApiHouses;
        }
    }
}
