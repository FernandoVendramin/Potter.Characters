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

        /// <summary>
        /// Retorna todos os Personagens.
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<DefaultResult<List<CharacterResponse>>>> Get(string id, string name, string house, string role, string school, string patronus)
        {
            var defaultResult = new DefaultResult<List<CharacterResponse>>();

            try
            {
                defaultResult = await _characterService.GetAsync(new CharacterRequestFilter()
                {
                    Id = id,
                    House = house,
                    Name = name,
                    Role = role,
                    School = school,
                    Patronus = patronus
                });
            }
            catch (System.Exception ex)
            {
                defaultResult.SetMessage(ex.Message);
                return StatusCode(500, defaultResult);
            }

            return defaultResult;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<DefaultResult<CharacterResponse>>> Insert([FromBody] CharacterRequest characterRequest)
        {
            var defaultResult = new DefaultResult<CharacterResponse>();

            try
            {
                defaultResult = await _characterService.InsertAsync(characterRequest);
            }
            catch (System.Exception ex)
            {
                defaultResult.SetMessage(ex.Message);
                return StatusCode(500, defaultResult);
            }

            return defaultResult;
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<DefaultResult<CharacterResponse>>> Update([FromBody] CharacterRequest characterRequest)
        {
            var defaultResult = new DefaultResult<CharacterResponse>();

            try
            {
                defaultResult = await _characterService.UpdateAsync(characterRequest);
            }
            catch (System.Exception ex)
            {
                defaultResult.SetMessage(ex.Message);
                return StatusCode(500, defaultResult);
            }

            return defaultResult;
        }

        [HttpDelete]
        [Route("")]
        public async Task<ActionResult<DefaultResultMessage>> Delete(string id)
        {
            var defaultResult = new DefaultResultMessage();

            try
            {
                defaultResult = await _characterService.DeleteAsync(id);
            }
            catch (System.Exception ex)
            {
                defaultResult.SetMessage(ex.Message);
                return StatusCode(500, defaultResult);
            }

            return defaultResult;
        }
    }
}
