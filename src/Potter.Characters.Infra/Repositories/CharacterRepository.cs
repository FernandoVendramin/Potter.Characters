using MongoDB.Driver;
using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Domain.Models;
using Potter.Characters.Infra.Interfaces;
using Potter.Characters.Infra.Repositories.Base;

namespace Potter.Characters.Infra.Repositories
{
    public class CharacterRepository : RepositoryBase<Character>, ICharacterRepository
    {
        public CharacterRepository(IMongoClient client, IMongoConfig config) : base(client, config)
        {

        }
    }
}
