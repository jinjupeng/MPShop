using ApiServer.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ApiServer.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextMySql myDbContext;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(ContextMySql myDbContext, ILogger<UnitOfWork> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        /// <summary>
        /// 获取 当前开启的事务
        /// </summary>
        public IDbContextTransaction CurrentTransaction { get; private set; }

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommitted { get; private set; }

        /// <summary>
        /// 获取 是否已启用事务
        /// </summary>
        public bool IsEnabledTransaction => CurrentTransaction != null;

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (!IsEnabledTransaction)
            {
                CurrentTransaction = myDbContext.Database.BeginTransaction();
            }
            HasCommitted = false;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (IsEnabledTransaction)
            {
                try
                {
                    CurrentTransaction.Commit();
                }
                catch(Exception ex)
                {
                    _logger.LogError("事务提交异常");
                    throw new Exception(ex.Message);
                }
            }
            HasCommitted = true;
        }

        public DbContext GetDbContext()
        {
            return myDbContext;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (IsEnabledTransaction)
            {
                CurrentTransaction.Rollback();
            }
            HasCommitted = true;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await myDbContext.SaveChangesAsync();
        }
    }
}
