using Potter.Characters.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potter.Characters.Domain.Interfaces
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetAllAsync();
        Task<Character> GetByNameAsync(string name);
        Task<bool> ExistsByName(string name);
        Task<Character> InsertAsync(Character character);
    }
}
