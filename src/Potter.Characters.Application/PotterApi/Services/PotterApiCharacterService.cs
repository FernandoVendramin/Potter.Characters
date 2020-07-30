using Potter.Characters.Application.PotterApi.Interfaces;
using Potter.Characters.Application.PotterApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Potter.Characters.Application.PotterApi.Services
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
        public async Task<List<PotterApiCharacter>> GetByName(string name)
        {
            return await _httpClient.GetFromJsonAsync<List<PotterApiCharacter>>(
                $"v1/characters?key={_potterApiConfig.Key}&name={name}");
        }
    }
}
