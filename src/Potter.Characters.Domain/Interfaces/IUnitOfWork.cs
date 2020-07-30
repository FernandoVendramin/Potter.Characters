using System.Threading.Tasks;

namespace Potter.Characters.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
