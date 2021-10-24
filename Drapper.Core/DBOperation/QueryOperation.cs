using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Drapper.Core.SqlStringHelper;

namespace Drapper.Core.DBOperation
{
    public partial class SqlOperation<TClass>
    {
        protected IEnumerable<TOut> Query<TIn, TOut>(TIn entity, Condition condition)
           => QueryImpl<TIn, TOut>(entity, condition);

        protected IEnumerable<TClass> Query<TIn>(TIn entity, Condition condition)
            => QueryImpl<TIn, TClass>(entity, condition);

        private IEnumerable<TOut> QueryImpl<TIn, TOut>(TIn entity, Condition condition)
        {
            var db = condition?.transaction?.Connection ?? GetDbConnection(ConnStr);

            if (condition.TableType == null)
                condition.WithTable<TClass>();

            var sql = SqlStringEngine.GenerateQuerySql(entity, condition);

            return db.Query<TOut>(sql, entity, condition?.transaction);
        }

        /// <summary>
        /// 暂时不开放此接口
        /// </summary>
        private IEnumerable<TOut> QuerySql<TIn, TOut>(string sql, TIn entity, string conn = null, IDbTransaction transaction = null)
        {
            var db = transaction?.Connection ?? GetDbConnection(conn);
            return db.Query<TOut>(sql, entity, transaction);
        }
    }
}
