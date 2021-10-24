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
        #region PageTable

        protected IEnumerable<TOut> Page<TIn, TOut>(TIn entity, PageCondition condition, out long totalCount)
           => PageImpl<TIn, TOut>(entity, condition, out totalCount);

        protected IEnumerable<TClass> Page<TIn>(TIn entity, PageCondition condition, out long totalCount)
            => PageImpl<TIn, TClass>(entity, condition, out totalCount);

        private IEnumerable<TOut> PageImpl<TIn, TOut>(TIn entity, PageCondition condition, out long totalCount)
        {
            var db = condition?.transaction?.Connection ?? GetDbConnection(ConnStr);

            if (condition.TableType == null)
                condition.WithTable<TClass>();

            var sql = SqlStringEngine.GeneratePageSql(entity, condition);

            var result = db.QueryMultiple(sql, entity, condition.transaction);
            var tmp = result.Read<TOut>();
            totalCount = result.Read<int>().SingleOrDefault();
            return tmp;
        }

        #endregion


        #region PageSql

        protected IEnumerable<TClass> PageSql<TIn>(string sql, TIn entity, out long totalCount, IDbTransaction dbTransaction = null) => PageSqlImpl<TIn, TClass>(sql, entity, out totalCount, ConnStr, dbTransaction);

        protected IEnumerable<TOut> PagesSql<TIn, TOut>(string sql, TIn entity, out long totalCount, string conn = null, IDbTransaction dbTransaction = null) => PageSqlImpl<TIn, TOut>(sql, entity, out totalCount, conn ?? ConnStr, dbTransaction);

        private IEnumerable<TOut> PageSqlImpl<TIn, TOut>(string sql, TIn entity, out long totalCount, string conn = null, IDbTransaction dbTransaction = null)
        {
            var db = dbTransaction?.Connection ?? GetDbConnection(conn);
            var result = db.QueryMultiple(sql, entity, dbTransaction);
            var tmp = result.Read<TOut>();
            totalCount = result.Read<int>().SingleOrDefault();
            return tmp;
        }

        #endregion

    }
}
