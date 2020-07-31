using MongoDB.Driver;
using Potter.Characters.Domain.Interfaces.Base;
using Potter.Characters.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Potter.Characters.Infra.Repositories.Base
{
    public abstract class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : IModelBase
    {
        private readonly IMongoCollection<TModel> _collection;

        public RepositoryBase(IMongoClient client, IMongoConfig mongoConfig)
        {
            var database = client.GetDatabase(mongoConfig.Database);
            _collection = database.GetCollection<TModel>(typeof(TModel).Name);
        }

        public Task<TModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TModel>> GetByFilterAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(Builders<TModel>.Filter.Empty).ToListAsync(cancellationToken);
        }

        public async Task<TModel> InsertAsync(TModel model, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(model, new InsertOneOptions(), cancellationToken);
            return model;
        }

        public Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
