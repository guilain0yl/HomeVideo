using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Drapper.Core.SqlStringHelper;

namespace Drapper.Core.DBOperation
{
    public partial class SqlOperation<TClass>
    {
        [Obsolete("This method will physically delete the data.")]
        protected bool Delete(TClass entity, Condition condition)
            => DeleteImpl(entity, condition);

        [Obsolete("This method will physically delete the data.")]
        protected bool Delete<TIn>(TIn entity, Condition condition)
            => DeleteImpl(entity, condition);

        private bool DeleteImpl<TIn>(TIn entity, Condition condition)
        {
            var db = condition?.transaction?.Connection ?? GetDbConnection(ConnStr);

            if (condition.TableType == null)
                condition.WithTable<TClass>();

            var sql = SqlStringEngine.GenerateDeleteSql(entity, condition);

            return db.Execute(sql, entity, condition?.transaction) > 0;
        }
    }
}
