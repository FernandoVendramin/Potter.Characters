using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Infra.Context;
using System.Threading.Tasks;

namespace Potter.Characters.Infra.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CommitAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
