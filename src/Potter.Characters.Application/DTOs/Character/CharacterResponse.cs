using Potter.Characters.Application.DTOs.House;

namespace Potter.Characters.Application.DTOs.Character
{
    public class CharacterResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string School { get; set; }
        public HouseResponse House { get; set; }
        public string Patronus { get; set; }

        public static explicit operator CharacterResponse(Domain.Models.Character character)
        {
            return new CharacterResponse()
            {
                Id = character.Id,
                Name = character.Name,
                Role = character.Role,
                School = character.School,
                House = (HouseResponse)character.House,
                Patronus = character.Patronus
            };
        }
    }
}
