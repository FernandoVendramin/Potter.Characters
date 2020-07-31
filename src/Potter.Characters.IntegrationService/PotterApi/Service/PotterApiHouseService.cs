using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Models;
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

        public PotterApiHouseService(IPotterApiConfig potterApiConfig, HttpClient httpClient)
        {
            _potterApiConfig = potterApiConfig;
            _httpClient = httpClient;
        }
        public async Task<List<PotterApiHouse>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PotterApiHouse>>(
                $"v1/houses?key={_potterApiConfig.Key}");
        }
    }
}
