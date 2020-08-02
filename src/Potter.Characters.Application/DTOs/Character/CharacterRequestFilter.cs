using MongoDB.Driver;
using System.Linq;

namespace Potter.Characters.Application.DTOs.Character
{
    public class CharacterRequestFilter
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string School { get; set; }
        public string House { get; set; }
        public string Patronus { get; set; }

        public FilterDefinition<Domain.Models.Character> GetFilterDefinition()
        {
            var builder = Builders<Domain.Models.Character>.Filter;
            var baseFilter = builder.Empty;

            if (!string.IsNullOrEmpty(House))
                baseFilter &= builder.Eq("House.Id", House);

            var props = typeof(CharacterRequestFilter).GetProperties().Where(x => x.Name != "House");
            foreach (var prop in props)
            {
                string value = prop.GetValue(this)?.ToString();
                if (!string.IsNullOrEmpty(value))
                    baseFilter &= builder.Eq(prop.Name, value);
            }

            return baseFilter;
        }
    }
}
