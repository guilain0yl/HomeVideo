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
        protected TOut Single<TIn, TOut>(TIn entity, Condition condition)
           => SingleColumnsImpl<TIn, TOut>(entity, condition);

        protected TClass Single<TIn>(TIn entity, Condition condition)
            => SingleColumnsImpl<TIn, TClass>(entity, condition);


        private TOut SingleColumnsImpl<TIn, TOut>(TIn entity, Condition condition)
        {
            var db = condition?.transaction?.Connection ?? GetDbConnection(ConnStr);

            if (condition.TableType == null)
                condition.WithTable<TClass>();

            var sql = SqlStringEngine.GenerateQuerySql(entity, condition);

            if (condition.SelectColumnsCount == 1 && typeof(TOut) != typeof(TClass))
            {
                return db.ExecuteScalar<TOut>(sql, entity, condition?.transaction);
            }

            return db.QueryFirstOrDefault<TOut>(sql, entity, condition?.transaction);
        }

    }
}
