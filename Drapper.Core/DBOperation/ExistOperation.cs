using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Drapper.Core.SqlStringHelper;

namespace Drapper.Core.DBOperation
{
    public partial class SqlOperation<TClass>
    {
        protected bool Exist(TClass entity, Condition condition)
           => ExistImpl(entity, condition);

        protected bool Exist<TIn>(TIn entity, Condition condition)
            => ExistImpl(entity, condition);

        private bool ExistImpl<TIn>(TIn entity, Condition condition)
        {
            var db = condition?.transaction?.Connection ?? GetDbConnection(ConnStr);

            if (condition.TableType == null)
                condition.WithTable<TClass>();

            var sql = SqlStringEngine.GenerateExistSql(entity, condition);

            return db.ExecuteScalar<int>(sql, entity, condition?.transaction) > 0;
        }
    }
}
