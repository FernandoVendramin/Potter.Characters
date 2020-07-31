using Microsoft.AspNetCore.Mvc;
using Potter.Characters.Application.DTOs;
using Potter.Characters.Application.DTOs.Character;
using Potter.Characters.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potter.Characters.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<CharacterResponse>>> GetAll()
        {
            return await _characterService.GetAllAsync();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<DefaultResult<CharacterResponse>>> Insert([FromBody] CharacterRequest characterRequest)
        {
            return await _characterService.InsertAsync(characterRequest);
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<DefaultResult<CharacterResponse>>> Update([FromBody] CharacterRequest characterRequest)
        {
            return await _characterService.UpdateAsync(characterRequest);
        }
    }
}
