using MongoDB.Driver;
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
using System.Linq;
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
        public async Task CharacterService_Delete_Success()
        {
            string id = "123";
            _characterRepositoryMock
                .Setup(x => x.DeleteAsync(id, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(Task.FromResult(true));

            var deleteResult = await _characterService.DeleteAsync(id);

            Assert.True(deleteResult.Success);
            Assert.Single(deleteResult.Messages);
            Assert.Contains(deleteResult.Messages, x => x == CommonMessages.ReccordsDeleted);

            _characterRepositoryMock.Verify(x =>
                x.DeleteAsync(id, It.IsAny<System.Threading.CancellationToken>()),
                Times.Once()
            );
        }

        [Fact]
        public async Task CharacterService_Delete_NotSuccess()
        {
            string id = "123";
            _characterRepositoryMock
                .Setup(x => x.DeleteAsync(id, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(Task.FromResult(false));

            var deleteResult = await _characterService.DeleteAsync(id);

            Assert.False(deleteResult.Success);
            Assert.Single(deleteResult.Messages);
            Assert.Contains(deleteResult.Messages, x => x == string.Format(CharacterMessages.NotExistsIdInDatabase, id));

            _characterRepositoryMock.Verify(x =>
                x.DeleteAsync(id, It.IsAny<System.Threading.CancellationToken>()),
                Times.Once()
            );
        }

        public static IEnumerable<object[]> GetValidCharacters_WithOrWithoutHouse
        {
            get
            {
                return new[]
                {
                     new object[] {
                         new Character(
                            "id1",
                            "name",
                            null,
                            null,
                            null,
                            null,
                            new System.DateTime(),
                            new System.DateTime()
                        )
                     },
                     new object[] {
                        new Character(
                            "id2",
                            "name",
                            null,
                            null,
                            new House("houseid", "name", "mascot", "hof", "hog", "founder", "school"),
                            null,
                            new System.DateTime(),
                            new System.DateTime()
                        )
                    }
                };
            }
        }

        [Theory(DisplayName = "CharacterService_Update_Success")]
        [MemberData(nameof(GetValidCharacters_WithOrWithoutHouse))]
        public async Task CharacterService_Update_Success(Character character)
        {
            MockDbCharacter_Success(character);

            var characterRequest = new CharacterRequest()
            {
                Name = character.Name,
                Patronus = character.Patronus,
                Role = character.Role,
                School = character.School,
                House = character.House?.Id
            };

            MockCheckHouseInconsistencies_Success(characterRequest, character.Id);

            var insertResult = await _characterService.UpdateAsync(characterRequest);

            Assert.True(insertResult.Success);

            _characterRepositoryMock.Verify(x =>
                x.UpdateAsync(
                  It.Is<Character>(y =>
                      y.Id == character.Id
                      && y.Name == character.Name
                      && y.Patronus == character.Patronus
                      && (character.House != null ? y.House.Id == character.House.Id : y.House == null)
                      && y.Role == character.Role
                      && y.School == character.School
                  ),
                  It.IsAny<System.Threading.CancellationToken>()
                ),
                Times.Once()
            );
        }

        public static IEnumerable<object[]> GetValidCharacterRequests_WithOrWithoutHouse
        {
            get
            {
                return new[]
                {
                     new object[] {
                        new CharacterRequest()
                        {
                            Name = "name",
                            Patronus = "pat",
                            Role = "role",
                            School = "scho"
                        }
                     },
                     new object[] {
                        new CharacterRequest()
                        {
                            Name = "name",
                            Patronus = "pat",
                            House = "houseid",
                            Role = "role",
                            School = "scho"
                        }
                     }
                };
            }
        }

        [Theory(DisplayName = "CharacterService_Insert_Success")]
        [MemberData(nameof(GetValidCharacterRequests_WithOrWithoutHouse))]
        public async Task CharacterService_Insert_Success(CharacterRequest characterRequest)
        {
            var potterCharacter = MockPotterCharacter_Success(characterRequest);
            MockCheckHouseInconsistencies_Success(characterRequest, potterCharacter._id);

            var insertResult = await _characterService.InsertAsync(characterRequest);

            Assert.True(insertResult.Success);

            _characterRepositoryMock.Verify(x =>
                x.InsertAsync(
                  It.Is<Character>(y =>
                      y.Name == characterRequest.Name
                      && y.Patronus == characterRequest.Patronus
                      && (characterRequest.House != null ? y.House.Id == characterRequest.House : y.House == null)
                      && y.Role == characterRequest.Role
                      && y.School == characterRequest.School
                  ), 
                  It.IsAny<System.Threading.CancellationToken>()
                ),
                Times.Once()
            );
        }

        [Fact]
        public async Task CharacterService_Insert_NotExistsInPotterApi_NotSuccess()
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
        public async Task CharacterService_Insert_HouseInconsistencies_NotSuccess()
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

        #region Aux Functions
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

        private void MockDbCharacter_Success(Character character)
        {
            List<Character> listCharacter = new List<Character>() { character };
            _characterRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<FilterDefinition<Character>>(),
                It.IsAny<System.Threading.CancellationToken>())
            )
            .Returns(Task.FromResult((IEnumerable<Character>)listCharacter));
        }

        private void MockCheckHouseInconsistencies_Success(CharacterRequest character, string id)
        {
            DefaultResult<House> houseResponse = new DefaultResult<House>();

            if (string.IsNullOrEmpty(character.House))
                houseResponse.SetData(null);
            else
                houseResponse.SetData(new House(character.House, null, null, null, null, null, null));

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
        #endregion
    }
}
