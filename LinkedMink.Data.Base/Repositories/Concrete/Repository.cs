using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LinkedMink.Data.Base.Entities;
using LinkedMink.Data.Base.Entities.Identity;

namespace LinkedMink.Data.Base.Repositories.Concrete
{
    public class Repository<TUser, TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntity
        where TUser : ClientUser
    {
        public Repository(BaseDbContext<TUser> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetAsync(long id)
        {
            return await EntityDbSet
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedData<TEntity>> GetAsync(PageCriteria criteria)
        {
            PagedData<TEntity> returnValue = new PagedData<TEntity>() { Criteria = criteria };
            IQueryable<TEntity> query = EntityDbSet;

            returnValue.NumberOfRecords = query.Count();

            PageData(query, criteria);

            returnValue.RecordsInPage = await query.ToListAsync();

            return returnValue;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(SortCriteria criteria = null)
        {
            IQueryable<TEntity> query = EntityDbSet;

            if (criteria != null)
                SortData(query, criteria);

            return await query.ToListAsync();
        }

        public async Task UpdateAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            EntityDbSet.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<TEntity> entities)
        {
            EntityDbSet.AddRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = EntityDbSet.First(e => e.Id == id);
            EntityDbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            EntityDbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<long> ids)
        {
            var entities = EntityDbSet.Where(e => ids.Contains(e.Id));
            EntityDbSet.RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            EntityDbSet.RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }

        protected DbSet<TEntity> EntityDbSet => _dbContext.GetDbSetByEntityType<TEntity>();

        protected BaseDbContext<TUser> DbContext => _dbContext;

        protected (IQueryable<TEntity> query, long numberOfRecords, int numberOfPages) PageData(
            IQueryable<TEntity> query,
            PageCriteria criteria)
        {
            if (criteria == null || criteria.PageSize <= 0)
                throw new ArgumentException("PageCriteria must have a PageSize greater than zero");

            var numberOfRecords = query.LongCount();

            if (criteria.Offset > 0)
                query = query.Skip(criteria.Offset);

            query = query.Take(criteria.PageSize);

            return (
                query: query,
                numberOfRecords: numberOfRecords, 
                numberOfPages: (int) Math.Ceiling((float) numberOfRecords / criteria.PageSize));
        }

        protected IQueryable<TEntity> SortData(
            IQueryable<TEntity> query, 
            SortCriteria criteria)
        {
            MethodInfo getMethod = criteria.Property?.GetGetMethod();

            if (getMethod != null)
            {
                query = criteria.Order == SortCriteria.SortOrder.Ascending
                    ? query.OrderBy(u => getMethod.Invoke(u, null))
                    : query.OrderByDescending(u => getMethod.Invoke(u, null));
            }

            return query;
        }

        private readonly BaseDbContext<TUser> _dbContext;
    }
}
