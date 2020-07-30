using Potter.Characters.IntegrationService.PotterApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potter.Characters.IntegrationService.PotterApi.Interfaces
{
    public interface IPotterApiCharacterService
    {
        Task<List<PotterApiCharacter>> GetByName(string name);
    }
}
