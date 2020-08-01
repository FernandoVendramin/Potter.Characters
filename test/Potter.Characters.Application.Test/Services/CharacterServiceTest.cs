using Moq;
using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.Application.Interfaces;
using Potter.Characters.Application.Services;
using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Domain.Models;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Models;
using Potter.Characters.Utils.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Potter.Characters.Application.Test.Services
{
    public class CharacterServiceTest
    {
        private readonly CharacterService _characterService;
        private readonly Mock<ICharacterRepository> _characterRepositoryMock;
        private readonly Mock<IPotterApiCharacterService> _potterApiCharacterServiceMock;
        private readonly Mock<IHouseService> _houseServiceMock;

        public CharacterServiceTest()
        {
            _characterRepositoryMock = new Mock<ICharacterRepository>();
            _potterApiCharacterServiceMock = new Mock<IPotterApiCharacterService>();
            _houseServiceMock = new Mock<IHouseService>();

            _characterService = new CharacterService(
                _characterRepositoryMock.Object,
                _potterApiCharacterServiceMock.Object,
                _houseServiceMock.Object);
        }

        [Fact]
        public async Task CharacterService_Insert_Success()
        {
            var character = new CharacterRequest()
            {
                Name = "name",
                Patronus = "pat",
                House = "houseid",
                Role = "role",
                School = "scho"
            };

            var potterCharacter = MockPotterCharacter_Success(character);
            MockCheckHouseInconsistencies_Success(character, potterCharacter._id);

            var insertResult = await _characterService.InsertAsync(character);

            Assert.True(insertResult.Success);

            _characterRepositoryMock.Verify(x =>
                x.InsertAsync(
                  It.Is<Character>(y =>
                      y.Name == character.Name
                      && y.Patronus == character.Patronus
                      && y.House.Id == character.House
                      && y.Role == character.Role
                      && y.School == character.School
                  ), default
                )
            );
        }

        [Fact]
        public async Task CharacterService_InsertNotExistsInPotterApi_NotSuccess()
        {
            var character = new CharacterRequest()
            {
                Name = "name",
                Patronus = "pat",
                House = "houseid",
                Role = "role",
                School = "scho"
            };

            MockPotterCharacter_NotSuccess();
            MockCheckHouseInconsistencies_NotSuccess();

            var insertResult = await _characterService.InsertAsync(character);

            Assert.False(insertResult.Success);
            Assert.NotEmpty(insertResult.Messages);
            Assert.Single(insertResult.Messages);
            Assert.Contains(insertResult.Messages, x => x == string.Format(CharacterMessages.NotExistsInPotterApi, character.Name));
        }

        [Fact]
        public async Task CharacterService_InsertHouseInconsistencies_NotSuccess()
        {
            var character = new CharacterRequest()
            {
                Name = "name",
                Patronus = "pat",
                House = "houseid",
                Role = "role",
                School = "scho"
            };

            MockPotterCharacter_Success(character);
            MockCheckHouseInconsistencies_NotSuccess();

            var insertResult = await _characterService.InsertAsync(character);

            Assert.False(insertResult.Success);
        }

        private PotterApiCharacter MockPotterCharacter_Success(CharacterRequest character)
        {
            var potterApiCharacter = new PotterApiCharacter()
            {
                _id = "charid",
                name = character.Name,
                patronus = character.Patronus,
                house = character.House,
                role = character.Role,
                school = character.School
            };
            List<PotterApiCharacter> potterApiCharacterList = new List<PotterApiCharacter>() { potterApiCharacter };
            _potterApiCharacterServiceMock.Setup(x => x.GetByNameAsync(character.Name)).Returns(Task.FromResult(potterApiCharacterList));

            return potterApiCharacter;
        }

        private void MockCheckHouseInconsistencies_Success(CharacterRequest character, string id)
        {
            DefaultResult<House> houseResponse = new DefaultResult<House>();
            houseResponse.SetData(new House("houseid", null, null, null, null, null, null));
            _houseServiceMock.Setup(x => x.CheckHouseInconsistenciesAsync(character, id)).Returns(Task.FromResult(houseResponse));
        }

        private void MockPotterCharacter_NotSuccess()
        {
            List<PotterApiCharacter> potterApiCharacterList = new List<PotterApiCharacter>();
            _potterApiCharacterServiceMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(potterApiCharacterList));
        }

        private void MockCheckHouseInconsistencies_NotSuccess()
        {
            DefaultResult<House> houseResponse = new DefaultResult<House>();
            houseResponse.IsNotSuccess();
            _houseServiceMock.Setup(x => x.CheckHouseInconsistenciesAsync
                (
                    It.IsAny<CharacterRequest>(),
                    It.IsAny<string>())
                )
                .Returns(Task.FromResult(houseResponse));
        }
    }
}
