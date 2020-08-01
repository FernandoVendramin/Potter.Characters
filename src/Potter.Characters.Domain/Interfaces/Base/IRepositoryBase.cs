using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Potter.Characters.Domain.Interfaces.Base
{
    public interface IRepositoryBase<TModel> where TModel : IModelBase
    {
        Task<IEnumerable<TModel>> GetAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default);
        Task<TModel> InsertAsync(TModel model, CancellationToken cancellationToken = default(CancellationToken));
        Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default(CancellationToken));
        Task<DeleteResult> DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
