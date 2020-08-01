using MongoDB.Driver;
using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.Application.Interfaces;
using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Domain.Models;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.Utils.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Potter.Characters.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IPotterApiCharacterService _potterApiCharacterService;
        private readonly IHouseService _houseService;

        public CharacterService(
            ICharacterRepository characterRepository,
            IPotterApiCharacterService potterApiCharacterService,
            IHouseService houseService)
        {
            _characterRepository = characterRepository;
            _potterApiCharacterService = potterApiCharacterService;
            _houseService = houseService;
        }

        public async Task<DefaultResult<List<CharacterResponse>>> GetAsync(CharacterRequestFilter filters)
        {
            var defaultResult = new DefaultResult<List<CharacterResponse>>();
            var baseFilter = GetFilterDefinition(filters);

            defaultResult.SetData((await _characterRepository.GetAsync(baseFilter)).Select(x => (CharacterResponse)x).ToList());
            return defaultResult;
        }

        public FilterDefinition<Character> GetFilterDefinition(CharacterRequestFilter filters)
        {
            var builder = Builders<Character>.Filter;
            var baseFilter = builder.Empty;

            if (!string.IsNullOrEmpty(filters.House))
                baseFilter &= builder.Eq("House.Id", filters.House);

            var props = typeof(CharacterRequestFilter).GetProperties().Where(x => x.Name != "House");
            foreach (var prop in props)
            {
                string value = prop.GetValue(filters)?.ToString();
                if (!string.IsNullOrEmpty(value))
                    baseFilter &= builder.Eq(prop.Name, value);
            }

            return baseFilter;
        }

        public async Task<bool> ExistsByName(string name)
        {
            var character = await _characterRepository.GetAsync(new FilterDefinitionBuilder<Character>().Where(x => x.Name == name));
            return character != null && character.Count() > 0;
        }

        private async Task<DefaultResult<CheckInsertInconsistenciesResponse>> CheckInsertInconsistenciesAsync(CharacterRequest characterRequest)
        {
            var defaultResult = new DefaultResult<CheckInsertInconsistenciesResponse>();

            var potterApiCharacter = (await _potterApiCharacterService.GetByNameAsync(characterRequest.Name)).FirstOrDefault();
            if (potterApiCharacter == null)
            {
                defaultResult.SetMessage(string.Format(CharacterMessages.NotExistsInPotterApi, characterRequest.Name));
                return defaultResult;
            }

            var houseInconsistencies = await _houseService.CheckHouseInconsistenciesAsync(characterRequest, potterApiCharacter._id);
            if (!houseInconsistencies.Success)
            {
                defaultResult.SetMessages(houseInconsistencies.Messages);
                return defaultResult;
            }

            defaultResult.SetData(new CheckInsertInconsistenciesResponse()
            {
                Id = potterApiCharacter._id,
                House = houseInconsistencies.Data
            });

            return defaultResult;
        }

        private async Task<DefaultResult<CheckUpdateInconsistenciesResponse>> CheckUpdateInconsistenciesAsync(CharacterRequest characterRequest)
        {
            var defaultResult = new DefaultResult<CheckUpdateInconsistenciesResponse>();

            var dbCharacter = (await _characterRepository.GetAsync
                (new FilterDefinitionBuilder<Character>().Where(x => x.Name == characterRequest.Name)))
                .FirstOrDefault();

            if (dbCharacter == null)
            {
                defaultResult.SetMessage(string.Format(CharacterMessages.NotExistsNameInDatabase, characterRequest.Name));
                return defaultResult;
            }

            var houseInconsistencies = await _houseService.CheckHouseInconsistenciesAsync(characterRequest, dbCharacter.Id);
            if (!houseInconsistencies.Success)
            {
                defaultResult.SetMessages(houseInconsistencies.Messages);
                return defaultResult;
            }

            defaultResult.SetData(new CheckUpdateInconsistenciesResponse()
            {
                Character = dbCharacter,
                House = houseInconsistencies.Data
            });

            return defaultResult;
        }

        public async Task<DefaultResult<CharacterResponse>> InsertAsync(CharacterRequest characterRequest)
        {
            var defaultResult = new DefaultResult<CharacterResponse>();

            var characterInconsistenciesResult = await CheckInsertInconsistenciesAsync(characterRequest);
            if (!characterInconsistenciesResult.Success)
            {
                defaultResult.SetMessages(characterInconsistenciesResult.Messages);
                return defaultResult;
            }

            var character = new Character(
                characterInconsistenciesResult.Data.Id,
                characterRequest.Name,
                characterRequest.Role,
                characterRequest.School,
                characterInconsistenciesResult.Data.House,
                characterRequest.Patronus,
                DateTime.Now,
                DateTime.Now);

            var validatorResult = character.Validate();

            if (!validatorResult.IsValid)
                defaultResult.SetMessages(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());
            else
                if ((await ExistsByName(character.Name)))
                defaultResult.SetMessage(string.Format(CharacterMessages.ExistsInDatabase, character.Name));

            if (defaultResult.Success)
            {
                await _characterRepository.InsertAsync(character);
                defaultResult.SetData((CharacterResponse)character);
                defaultResult.IsSuccess();
            }

            return defaultResult;
        }

        public async Task<DefaultResult<CharacterResponse>> UpdateAsync(CharacterRequest characterRequest)
        {
            var defaultResult = new DefaultResult<CharacterResponse>();

            var characterInconsistenciesResult = await CheckUpdateInconsistenciesAsync(characterRequest);
            if (!characterInconsistenciesResult.Success)
            {
                defaultResult.SetMessages(characterInconsistenciesResult.Messages);
                return defaultResult;
            }

            var character = new Character(
                characterInconsistenciesResult.Data.Character.Id,
                characterRequest.Name,
                characterRequest.Role,
                characterRequest.School,
                characterInconsistenciesResult.Data.House,
                characterRequest.Patronus,
                DateTime.Now,
                characterInconsistenciesResult.Data.Character.CreationDateTime);

            var validatorResult = character.Validate();

            if (!validatorResult.IsValid)
                defaultResult.SetMessages(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());

            if (defaultResult.Success)
            {
                await _characterRepository.UpdateAsync(character);
                defaultResult.SetData((CharacterResponse)character);
                defaultResult.IsSuccess();
            }

            return defaultResult;
        }

        public async Task<DefaultResultMessage> DeleteAsync(string id)
        {
            var deleteResult = await _characterRepository.DeleteAsync(id);
            var defaultResult = new DefaultResultMessage();

            if (deleteResult)
            {
                defaultResult.SetMessage(CommonMessages.ReccordsDeleted);
                defaultResult.IsSuccess();
            }
            else
                defaultResult.SetMessage(string.Format(CharacterMessages.NotExistsIdInDatabase, id));

            return defaultResult;
        }
    }
}
