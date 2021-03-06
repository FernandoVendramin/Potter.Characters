﻿using MongoDB.Driver;
using Potter.Characters.Domain.Interfaces.Base;
using Potter.Characters.Infra.Interfaces;
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

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = new FilterDefinitionBuilder<TModel>().Where(x => x.Id == id);
            var deleteResult = await _collection.DeleteOneAsync(filter, cancellationToken);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<TModel>> GetAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<TModel> InsertAsync(TModel model, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(model, new InsertOneOptions(), cancellationToken);
            return model;
        }

        public async Task UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            var filter = new FilterDefinitionBuilder<TModel>().Where(x => x.Id == model.Id);
            await _collection.ReplaceOneAsync(filter, model, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
    }
}
