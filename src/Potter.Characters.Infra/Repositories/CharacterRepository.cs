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
        //private readonly IMongoCollection<Character> _collection;
        //public CharacterRepository(IMongoClient client, IMongoConfig mongoConfig)
        //{
        //    var database = client.GetDatabase(mongoConfig.Database);
        //    _collection = database.GetCollection<Character>(typeof(Character).Name);
        //}

        //public Task<Character> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<Character>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IEnumerable<Character>> GetByFilterAsync(FilterDefinition<Character> filter, CancellationToken cancellationToken = default)
        //{
        //    return await _collection.Find(filter).ToListAsync(cancellationToken);
        //}

        //public Task<Character> InsertAsync(Character entity, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Character> UpdateAsync(Character entity, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
