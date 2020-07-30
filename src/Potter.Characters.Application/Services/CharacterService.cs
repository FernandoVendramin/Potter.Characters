using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.Interfaces;
using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Domain.Models;
using Potter.Characters.Domain.Validators;
using Potter.Characters.Utils.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Potter.Characters.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await _characterRepository.GetAllAsync();
        }

        public async Task<Character> GetByNameAsync(string name)
        {
            return await _characterRepository.GetByNameAsync(name);
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await _characterRepository.ExistsByName(name);
        }

        public async Task<DefaultResult<Character>> InsertAsync(CharacterNew characterNew)
        {
            var defaultResult = new DefaultResult<Character>();

            var character = new Character(characterNew.Name, characterNew.Role, characterNew.School, characterNew.House, characterNew.Patronus);
            var validator = new CharacterValidator();
            var validatorResult = validator.Validate(character);

            if (!validatorResult.IsValid)
                defaultResult.SetMessages(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());
            else
                if ((await ExistsByName(character.Name)))
                    defaultResult.SetMessage(string.Format(CharacterMessages.ExistsInDatabase, character.Name));

            if (defaultResult.Success)
                defaultResult.SetData(await _characterRepository.InsertAsync(character));

            return defaultResult;
        }
    }
}
