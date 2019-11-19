using LinkedMink.Data.Base.Repositories.Concrete;
using LinkedMink.Data.Base.Entities;
using LinkedMink.Data.Base.Entities.Identity;

namespace LinkedMink.Data.Domain.EnvironManager.Repositories.Concrete
{
    public class EnvironManagerRepository<TEntity> : Repository<ClientUser, TEntity> 
        where TEntity : class, IEntity
    {
        public EnvironManagerRepository(EnvironManagerDbContext context) : base(context)
        {

        }
    }
}
