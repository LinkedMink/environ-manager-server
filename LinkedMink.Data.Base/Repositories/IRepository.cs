using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedMink.Data.Base.Entities;

namespace LinkedMink.Data.Base.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetAsync(long id);
        Task<PagedData<TEntity>> GetAsync(PageCriteria criteria);
        Task<IEnumerable<TEntity>> GetAsync(SortCriteria criteria = null);
        Task UpdateAsync();
        Task AddAsync(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(long id);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(IEnumerable<long> ids);
        Task DeleteAsync(IEnumerable<TEntity> entities);
    }
}
