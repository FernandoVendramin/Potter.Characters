using Potter.Characters.Application.PotterApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potter.Characters.Application.PotterApi.Interfaces
{
    public interface IPotterApiCharacterService 
    {
        Task<List<PotterApiCharacter>> GetByName(string name);
    }
}
