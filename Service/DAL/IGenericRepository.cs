﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Service.DAL
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);               
        Task<IPagedList<TEntity>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);
        Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter,
            string includeProperties = null);
        ValueTask<TEntity> GetByIdAsync(object id);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
        Task InsertAsync(TEntity entity);
        Task DeleteAsync(TEntity entityToDelete);
        Task UpdateAsync(TEntity entityToUpdate);
    }
}
