using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Potter.Characters.IntegrationService.PotterApi.Service
{
    public class PotterApiCharacterService : IPotterApiCharacterService
    {
        private readonly IPotterApiConfig _potterApiConfig;
        private readonly HttpClient _httpClient;

        public PotterApiCharacterService(IPotterApiConfig potterApiConfig, HttpClient httpClient)
        {
            _potterApiConfig = potterApiConfig;
            _httpClient = httpClient;
        }
        public async Task<List<PotterApiCharacter>> GetByNameAsync(string name)
        {
            return await _httpClient.GetFromJsonAsync<List<PotterApiCharacter>>(
                $"v1/characters?key={_potterApiConfig.Key}&name={name}");
        }
    }
}
