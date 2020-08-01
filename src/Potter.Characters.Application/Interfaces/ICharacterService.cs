using MongoDB.Driver;
using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potter.Characters.Application.Interfaces
{
    public interface ICharacterService
    {
        Task<DefaultResult<List<CharacterResponse>>> GetAllAsync(CharacterRequestFilter filters);
        Task<DefaultResult<List<CharacterResponse>>> GetByFilterAsync(FilterDefinition<Character> filter);
        Task<bool> ExistsByName(string name);
        Task<DefaultResult<CharacterResponse>> InsertAsync(CharacterRequest characterRequest);
        Task<DefaultResult<CharacterResponse>> UpdateAsync(CharacterRequest characterRequest);
        Task<DefaultResultMessage> DeleteAsync(string id);
    }
}
