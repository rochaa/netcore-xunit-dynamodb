using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gidu.Domain.Helpers
{
    public interface IRepository<TEntity>
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(string id);

        Task<List<TEntity>> GetBySearchFieldAsync(string search);

        Task SaveAsync(TEntity entidade);

        Task DeleteAsync(TEntity entidade);
    }
}