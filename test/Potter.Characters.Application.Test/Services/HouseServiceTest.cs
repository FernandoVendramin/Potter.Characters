using Moq;
using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.Application.Services;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.IntegrationService.PotterApi.Models;
using Potter.Characters.Utils.Messages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Potter.Characters.Application.Test.Services
{
    public class HouseServiceTest
    {
        private readonly HouseService _houseService;
        private readonly Mock<IPotterApiHouseService> _potterApiHouseServiceMock;

        public HouseServiceTest()
        {
            _potterApiHouseServiceMock = new Mock<IPotterApiHouseService>();
            _houseService = new HouseService(_potterApiHouseServiceMock.Object);
        }

        [Fact]
        public async Task HouseService_CheckHouseInconsistencies_Success()
        {
            var characterId = "characterId";
            var houseId = "houseId";
            PotterApiHouse house = new PotterApiHouse()
            {
                _id = houseId,
                founder = "founder",
                name = "name",
                members = new string[] { characterId }
            };
            List<PotterApiHouse> potterApiHouses = new List<PotterApiHouse>() { house };

            _potterApiHouseServiceMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(potterApiHouses));

            var character = new CharacterRequest()
            {
                Name = "name",
                Patronus = "pat",
                House = houseId,
                Role = "role",
                School = "scho"
            };

            var insertResult = await _houseService.CheckHouseInconsistenciesAsync(character, characterId);

            Assert.True(insertResult.Success);
            Assert.Equal(insertResult.Data.Id, house._id);
            Assert.Equal(insertResult.Data.Founder, house.founder);
            Assert.Equal(insertResult.Data.Name, house.name);
            Assert.Equal(insertResult.Data.Mascot, house.mascot);
            Assert.Equal(insertResult.Data.HeadOfHouse, house.headOfHouse);
            Assert.Equal(insertResult.Data.HouseGhost, house.houseGhost);
        }

        [Fact]
        public async Task HouseService_CheckHouseInconsistenciesNotExistsInPotterApi_NotSuccess()
        {
            var characterId = "characterId";
            var houseId = "houseId";
            List<PotterApiHouse> potterApiHouses = new List<PotterApiHouse>();
            _potterApiHouseServiceMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(potterApiHouses));

            var character = new CharacterRequest()
            {
                Name = "name",
                Patronus = "pat",
                House = houseId,
                Role = "role",
                School = "scho"
            };

            var insertResult = await _houseService.CheckHouseInconsistenciesAsync(character, characterId);

            Assert.False(insertResult.Success);
            Assert.Single(insertResult.Messages);
            Assert.Contains(insertResult.Messages, x => x == string.Format(HouseMessages.NotExistsInPotterApi, character.House));
        }

        [Fact]
        public async Task HouseService_CheckHouseInconsistenciesCharacterNotPartOfHouse_NotSuccess()
        {
            var characterId = "characterId";
            var houseId = "houseId";
            PotterApiHouse house = new PotterApiHouse()
            {
                _id = houseId,
                founder = "founder",
                name = "name",
                members = new string[] {  }
            };
            List<PotterApiHouse> potterApiHouses = new List<PotterApiHouse>() { house };

            _potterApiHouseServiceMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(potterApiHouses));

            var character = new CharacterRequest()
            {
                Name = "name",
                Patronus = "pat",
                House = houseId,
                Role = "role",
                School = "scho"
            };

            var insertResult = await _houseService.CheckHouseInconsistenciesAsync(character, characterId);

            Assert.False(insertResult.Success);
            Assert.Single(insertResult.Messages);
            Assert.Contains(insertResult.Messages, x => x == string.Format(HouseMessages.CharacterNotPartOfHouse, character.Name, house.name));
        }
    }
}
