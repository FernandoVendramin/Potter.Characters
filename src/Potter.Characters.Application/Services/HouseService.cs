using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.Application.Interfaces;
using Potter.Characters.Domain.Models;
using Potter.Characters.IntegrationService.PotterApi.Interfaces;
using Potter.Characters.Utils.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace Potter.Characters.Application.Services
{
    public class HouseService : IHouseService
    {
        private readonly IPotterApiHouseService _potterApiHouseService;

        public HouseService(IPotterApiHouseService potterApiHouseService)
        {
            _potterApiHouseService = potterApiHouseService;
        }

        public async Task<DefaultResult<House>> CheckHouseInconsistenciesAsync(CharacterRequest characterRequest, string characterId)
        {
            var defaultResult = new DefaultResult<House>();

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
                    if (!potterHouse.members.Any(x => x == characterId))
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
                        potterHouse.founder);
                }
            }

            defaultResult.SetData(house);

            return defaultResult;
        }
    }
}
