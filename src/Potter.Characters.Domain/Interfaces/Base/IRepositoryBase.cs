using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Potter.Characters.Domain.Interfaces.Base
{
    public interface IRepositoryBase<TModel> where TModel : IModelBase
    {
        Task<IEnumerable<TModel>> GetByFilterAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<TModel> InsertAsync(TModel model, CancellationToken cancellationToken = default(CancellationToken));
        Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default(CancellationToken));
        Task<TModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
