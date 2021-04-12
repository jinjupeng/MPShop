using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ApiServer.DAL.IDAL
{
    public interface IDapperDal
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        DbConnection GetDbconnection();


        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        int Delete(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
