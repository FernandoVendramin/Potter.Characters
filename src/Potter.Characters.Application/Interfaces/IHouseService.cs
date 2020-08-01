using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.Domain.Models;
using System.Threading.Tasks;

namespace Potter.Characters.Application.Interfaces
{
    public interface IHouseService
    {
        Task<DefaultResult<House>> CheckHouseInconsistenciesAsync(CharacterRequest characterRequest, string characterId);
    }
}
