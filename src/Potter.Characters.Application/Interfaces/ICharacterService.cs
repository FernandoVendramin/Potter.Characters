using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potter.Characters.Application.Interfaces
{
    public interface ICharacterService
    {
        Task<List<CharacterResponse>> GetAllAsync();
        Task<CharacterResponse> GetByNameAsync(string name);
        Task<bool> ExistsByName(string name);
        Task<DefaultResult<CharacterResponse>> InsertAsync(CharacterRequest characterNew);
    }
}
