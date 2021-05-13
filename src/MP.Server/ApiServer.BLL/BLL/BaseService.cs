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
    /// <summary>
    /// 在Service层，对数据库的非查询操作都需要调用SaveChange()/SaveChangeAsync()方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

        public async Task<bool> AddRangeAsync(IEnumerable<T> t)
        {
            await _baseDal.AddRangeAsync(t);
            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public bool DeleteRange(IEnumerable<T> t)
        {
            _baseDal.DeleteRange(t);
            return unitOfWork.SaveChanges() > 0;
        }

        public bool DeleteRange(Expression<Func<T, bool>> expression)
        {
            _baseDal.DeleteRange(expression);
            return unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateRange(IEnumerable<T> t)
        {
            _baseDal.UpdateRange(t);
            return unitOfWork.SaveChanges() > 0;
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

        public async Task<int> InsertAsync(T entity)
        {
            await _baseDal.InsertAsync(entity);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
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
            var result = await _baseDal.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _baseDal.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _baseDal.RemoveAsync(entity);
            return await unitOfWork.SaveChangesAsync() > 0;
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
            _baseDal.DeleteRange(delModels);
            return unitOfWork.SaveChanges() > 0;
        }

        /// <summary>
        /// 同步-新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            var result = _baseDal.Add(entity);
            unitOfWork.SaveChanges();
            return result;
        }

        public bool Insert(T entity)
        {
            _baseDal.Add(entity);
            return unitOfWork.SaveChanges() > 0;
        }

        /// <summary>
        /// 同步-更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            _baseDal.Update(entity);
            return unitOfWork.SaveChanges() > 0;
        }

        /// <summary>
        /// 同步-删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Remove(T entity)
        {
            _baseDal.Remove(entity);
            return unitOfWork.SaveChanges() > 0;
        }
    }
}