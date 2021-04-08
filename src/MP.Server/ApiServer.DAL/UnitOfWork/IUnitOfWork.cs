using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace ApiServer.DAL.UnitOfWork
{
    /// <summary>
    /// 定义一个单元操作内的功能，管理单元操作内涉及的所有上下文对象及其事务
    /// </summary>
    public interface IUnitOfWork
    {
        IDbContextTransaction CurrentTransaction { get; }

        /// <summary>
        /// 获取 是否已提交
        /// </summary>
        bool HasCommitted { get; }

        /// <summary>
        /// 获取 是否启用事务
        /// </summary>
        bool IsEnabledTransaction { get; }

        /// <summary>
        /// 启用事务，事务代码写在 UnitOfWork.EnableTransaction() 与 UnitOfWork.Commit() 之间
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        void Rollback();

        Task<int> SaveChangesAsync();
    }
}
