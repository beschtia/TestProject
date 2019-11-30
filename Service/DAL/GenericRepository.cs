using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Service.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ProjectContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(ProjectContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<TEntity>();
        }

        private IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            includeProperties ??= string.Empty;
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);

            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public Task<List<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            return GetQueryable(null, orderBy, includeProperties).AsNoTracking().ToListAsync();
        }
        
        public async Task<IPagedList<TEntity>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            var count = await GetQueryable(filter).CountAsync();
            var entities = await GetQueryable(filter, orderBy, includeProperties,
                pageSize * (pageNumber - 1), pageSize).AsNoTracking().ToListAsync();
            return new StaticPagedList<TEntity>(entities, pageNumber, pageSize, count);
        }

        public Task<TEntity> GetOneAsync(
        Expression<Func<TEntity, bool>> filter,
        string includeProperties = null)
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
        }

        public ValueTask<TEntity> GetByIdAsync(object id)
        {            
            return dbSet.FindAsync(id);
        }

        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).CountAsync();
        }

        public Task InsertAsync(TEntity entity)
        {            
            dbSet.Add(entity);
            return dbContext.SaveChangesAsync();
        }
        
        public Task DeleteAsync(TEntity entityToDelete)
        {
            dbSet.Remove(entityToDelete);
            return dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            return dbContext.SaveChangesAsync();
        }
    }
}
