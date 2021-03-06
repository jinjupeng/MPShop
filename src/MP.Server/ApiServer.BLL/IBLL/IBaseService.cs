using ApiServer.Model.Model.PageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.BLL.IBLL
{
    public interface IBaseService<T> where T : class
    {
        Task<bool> AddRangeAsync(IEnumerable<T> t);
        bool DeleteRange(IEnumerable<T> t);
        bool DeleteRange(Expression<Func<T, bool>> expression);
        bool UpdateRange(IEnumerable<T> t);

        /// <summary>
        /// 根据whereLambda获取IQueryable集合
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);

        PageModel<T> QueryByPage<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy);

        Task<int> InsertAsync(T entity);

        Task<int> UpdateAsync(T entity);

        Task<bool> IsExistAsync(Expression<Func<T, bool>> whereLambda);

        Task<T> GetEntityAsync(Expression<Func<T, bool>> whereLambda);

        Task<List<T>> SelectAsync();

        Task<List<T>> SelectAsync(Expression<Func<T, bool>> whereLambda);

        Task<Tuple<List<T>, int>> SelectAsync<S>(int pageSize, int pageIndex, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(Expression<Func<T, bool>> express, CancellationToken cancellationToken = default);

        T Add(T entity);

        bool Insert(T entity);

        bool Update(T entity);

        bool Remove(T entity);

    }
}