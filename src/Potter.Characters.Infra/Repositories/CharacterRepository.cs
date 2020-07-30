using Microsoft.EntityFrameworkCore;
using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Potter.Characters.Infra.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        protected readonly DataContext _dataContext;
        public CharacterRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await _dataContext.Character.ToListAsync();
        }

        public async Task<Character> GetByNameAsync(string name)
        {
            return await _dataContext.Character.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await _dataContext.Character.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<Character> InsertAsync(Character character)
        {
            _dataContext.Character.Add(character);
            await _dataContext.SaveChangesAsync(); // REMOVER DPS

            return character;
        }
    }
}
