using ApiServer.BLL.IBLL;
using ApiServer.DAL.IDAL;
using ApiServer.DAL.UnitOfWork;
using ApiServer.Model.Model.PageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServer.BLL.BLL
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseDal<T> _baseDal;
        protected IUnitOfWork unitOfWork;

        public BaseService() { }
        public BaseService(IBaseDal<T> baseDal, IUnitOfWork unitOfWork)
        {
            _baseDal = baseDal;
            this.unitOfWork = unitOfWork;
        }

        public bool AddRange(params T[] t)
        {
            _baseDal.AddRange(t);
            return _baseDal.SaveChangesAsync();
        }

        public bool DeleteRange(IEnumerable<T> t)
        {
            _baseDal.DeleteRange(t);
            return _baseDal.SaveChangesAsync();
        }

        public bool DeleteRange(params T[] t)
        {
            _baseDal.DeleteRange(t);
            return _baseDal.SaveChangesAsync();
        }

        public bool UpdateRange(params T[] t)
        {
            _baseDal.UpdateRange(t);
            return _baseDal.SaveChangesAsync();
        }

        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return _baseDal.GetModels(whereLambda);
        }

        public PageModel<T> QueryByPage<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy)
        {
            PageModel<T> pageModel = new PageModel<T>
            {
                pageNum = pageIndex,
                size = pageSize,
                records = _baseDal.QueryByPage(pageIndex, pageSize, whereLambda, orderBy).ToList()
            };
            pageModel.total = pageModel.records.Count;
            pageModel.pageSize = pageModel.total % pageSize > 0 ? pageModel.total / pageSize + 1 : pageModel.total / pageSize;

            return pageModel;
        }

        public async Task<int> InsertAndSaveAsync(T entity)
        {
            await _baseDal.InsertAsync(entity);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateAndSaveAsync(T entity)
        {
            _baseDal.Update(entity);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _baseDal.IsExistAsync(whereLambda);
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _baseDal.GetEntityAsync(whereLambda);
        }

        public async Task<List<T>> SelectAsync()
        {
            return await _baseDal.SelectAsync();
        }

        public async Task<List<T>> SelectAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _baseDal.SelectAsync(whereLambda);
        }

        public async Task<Tuple<List<T>, int>> SelectAsync<S>(int pageSize, int pageIndex, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc)
        {
            return await _baseDal.SelectAsync(pageSize, pageIndex, whereLambda, orderByLambda, isAsc);
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await _baseDal.AddAsync(entity);
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await _baseDal.UpdateAsync(entity);
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await _baseDal.RemoveAsync(entity);
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(Expression<Func<T, bool>> express, CancellationToken cancellationToken = default)
        {
            var delModels = await _baseDal.SelectAsync(express);
            return await _baseDal.RemoveAsync(delModels.FirstOrDefault());
        }

        /// <summary>
        /// 同步-新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            return _baseDal.Add(entity);
        }

        /// <summary>
        /// 同步-更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Update(T entity)
        {
            return _baseDal.Update(entity);
        }

        /// <summary>
        /// 同步-删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Remove(T entity)
        {
            return _baseDal.Remove(entity);
        }
    }
}