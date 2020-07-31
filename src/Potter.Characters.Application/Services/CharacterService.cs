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
        private readonly IPotterApiHouseService _potterApiHouseService;

        public CharacterService(
            ICharacterRepository characterRepository,
            IPotterApiCharacterService potterApiCharacterService,
            IPotterApiHouseService potterApiHouseService)
        {
            _characterRepository = characterRepository;
            _potterApiCharacterService = potterApiCharacterService;
            _potterApiHouseService = potterApiHouseService;
        }

        public async Task<List<CharacterResponse>> GetAllAsync()
        {
            return (await _characterRepository.GetAllAsync()).Select(x => (CharacterResponse)x).ToList();
        }

        public async Task<List<CharacterResponse>> GetByFilterAsync(FilterDefinition<Character> filter)
        {
            return (await _characterRepository.GetByFilterAsync(filter)).Select(x => (CharacterResponse)x).ToList();
        }

        public async Task<bool> ExistsByName(string name)
        {
            var character = await _characterRepository.GetByFilterAsync(new FilterDefinitionBuilder<Character>().Where(x => x.Name == name));
            return character != null;
        }

        public async Task<DefaultResult<CharacterResponse>> InsertAsync(CharacterRequest characterRequest)
        {
            var defaultResult = new DefaultResult<CharacterResponse>();

            var potterApiCharacter = (await _potterApiCharacterService.GetByNameAsync(characterRequest.Name)).FirstOrDefault();
            if (potterApiCharacter == null)
            {
                defaultResult.SetMessage(string.Format(CharacterMessages.NotExistsInPotterApi, characterRequest.Name));
                return defaultResult;
            }

            House house = null;
            if (!string.IsNullOrEmpty(characterRequest.House))
            {
                var potterApiHouses = await _potterApiHouseService.GetAllAsync();
                var potterHouse = potterApiHouses.Where(x => x._id == characterRequest.House).FirstOrDefault();
                if (potterHouse == null)
                {
                    defaultResult.SetMessage(string.Format(HouseMessages.NotExistsInPotterApi, characterRequest.House));
                    return defaultResult;
                }
                else
                {
                    if (!potterHouse.members.Any(x => x == potterApiCharacter._id))
                    {
                        defaultResult.SetMessage(string.Format(HouseMessages.CharacterNotPartOfHouse, characterRequest.Name, potterHouse.name));
                        return defaultResult;
                    }

                    house = new House(
                        potterHouse._id,
                        potterHouse.name,
                        potterHouse.mascot,
                        potterHouse.headOfHouse,
                        potterHouse.houseGhost,
                        potterHouse.founder,
                        potterHouse.school);
                }
            }

            var character = new Character(
                potterApiCharacter._id,
                characterRequest.Name,
                characterRequest.Role,
                characterRequest.School,
                house,
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

            var dbCharacter = (await _characterRepository.GetByFilterAsync
                (new FilterDefinitionBuilder<Character>().Where(x => x.Name == characterRequest.Name)))
                .FirstOrDefault();

            if (dbCharacter == null)
            {
                defaultResult.SetMessage(string.Format(CharacterMessages.NotExistsInDatabase, characterRequest.Name));
                return defaultResult;
            }

            House house = dbCharacter.House;
            if (!string.IsNullOrEmpty(characterRequest.House))
            {
                var potterApiHouses = await _potterApiHouseService.GetAllAsync();
                var potterHouse = potterApiHouses.Where(x => x._id == characterRequest.House).FirstOrDefault();
                if (potterHouse == null)
                {
                    defaultResult.SetMessage(string.Format(HouseMessages.NotExistsInPotterApi, characterRequest.House));
                    return defaultResult;
                }
                else
                {
                    if (!potterHouse.members.Any(x => x == dbCharacter.Id))
                    {
                        defaultResult.SetMessage(string.Format(HouseMessages.CharacterNotPartOfHouse, characterRequest.Name, potterHouse.name));
                        return defaultResult;
                    }

                    house = new House(
                        potterHouse._id,
                        potterHouse.name,
                        potterHouse.mascot,
                        potterHouse.headOfHouse,
                        potterHouse.houseGhost,
                        potterHouse.founder,
                        potterHouse.school);
                }
            }
            else
                house = null;

            var character = new Character(
                dbCharacter.Id,
                characterRequest.Name,
                characterRequest.Role,
                characterRequest.School,
                house,
                characterRequest.Patronus,
                DateTime.Now,
                dbCharacter.CreationDateTime);

            var validatorResult = character.Validate();

            if (!validatorResult.IsValid)
                defaultResult.SetMessages(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());

            if (defaultResult.Success)
            {
                character = await _characterRepository.UpdateAsync(character);
                defaultResult.SetData((CharacterResponse)character);
                defaultResult.IsSuccess();
            }

            return defaultResult;
        }

        public async Task<DefaultResult<DeleteResult>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
