using Potter.Characters.Application.DTOs;
using Potter.Characters.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potter.Characters.Application.Interfaces
{
    public interface ICharacterService
    {
        Task<List<Character>> GetAllAsync();
        Task<Character> GetByNameAsync(string name);
        Task<bool> ExistsByName(string name);
        Task<DefaultResult<Character>> InsertAsync(CharacterNew characterNew);
    }
}
