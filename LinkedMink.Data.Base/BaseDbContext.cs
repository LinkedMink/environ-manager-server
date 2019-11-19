using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LinkedMink.Base.Extensions;
using LinkedMink.Data.Base.Entities;
using LinkedMink.Data.Base.Entities.Identity;

namespace LinkedMink.Data.Base
{
    public abstract class BaseDbContext<TUser> : 
        IdentityDbContext<TUser, ClientRole, long, ClientUserClaim, ClientUserRole, ClientUserLogin, ClientRoleClaim, ClientUserToken> 
        where TUser : ClientUser
    {
        protected BaseDbContext(DbContextOptions options) : base(options) { }

        public string ContextUser { get; set; } = "System Process";

        public DbSet<TEntity> GetDbSetByEntityType<TEntity>() where TEntity : class
        {
            DbSetPropertyByEntity.TryGetValue(typeof(TEntity), out var dbSetProperty);
            if (dbSetProperty != null)
                return (DbSet<TEntity>) dbSetProperty.GetValue(this);

            throw new NotSupportedException($"There is no DbSet mapping in DbSetPropertyByEntity for type {typeof(TEntity).Name}");
        }

        public override int SaveChanges()
        {
            UpdateTrackedEntityBeforeSave();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateTrackedEntityBeforeSave();

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected static readonly Dictionary<Type, PropertyInfo> DbSetPropertyByEntity = new Dictionary<Type, PropertyInfo>
        {
            { typeof(ClientRole), TypeHelpers.GetProperty<BaseDbContext<TUser>>(t => t.Roles) },
            { typeof(TUser), TypeHelpers.GetProperty<BaseDbContext<TUser>>(t => t.Users) },
        };

        protected virtual void UpdateTrackedEntityBeforeSave()
        {
            foreach (var entityEntry in ChangeTracker.Entries())
            {
                if (entityEntry.Entity is TrackedEntity entity)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        entity.SetCreatedBy(ContextUser);
                    }
                    else if (entityEntry.State == EntityState.Deleted || entityEntry.State == EntityState.Modified)
                    {
                        entity.UpdateModifiedBy(ContextUser);
                    }
                }
            }
        }
    }
}
