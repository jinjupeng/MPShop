using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.DAL.IDAL
{
    public interface IBaseDal<T> where T : class
    {
        void AddRange(IEnumerable<T> t);
        void AddRange(params T[] t);
        void DeleteRange(IEnumerable<T> t);
        void DeleteRange(params T[] t);
        void UpdateRange(IEnumerable<T> t);
        void UpdateRange(params T[] t);
        IQueryable<T> ExecSql(string sql);

        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> QueryByPage<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy);

        bool SaveChangesAsync();

        T Add(T entity);

        T Update(T entity);

        bool Remove(T entity);
        ValueTask<EntityEntry<T>> InsertAsync(T entity);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(T entity);

        Task<bool> IsExistAsync(Expression<Func<T, bool>> whereLambda);

        Task<T> GetEntityAsync(Expression<Func<T, bool>> whereLambda);

        Task<List<T>> SelectAsync();

        Task<List<T>> SelectAsync(Expression<Func<T, bool>> whereLambda);

        Task<Tuple<List<T>, int>> SelectAsync<S>(int pageSize, int pageIndex, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc);
    }
}