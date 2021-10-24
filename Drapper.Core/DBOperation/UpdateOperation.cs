using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Drapper.Core.SqlStringHelper;

namespace Drapper.Core.DBOperation
{
    public partial class SqlOperation<TClass>
    {
        protected int Update(TClass entity, Condition condition)
           => UpdateImpl(entity, condition);

        protected int Update<TIn>(TIn entity, Condition condition)
            => UpdateImpl(entity, condition);

        private int UpdateImpl<TIn>(TIn entity, Condition condition)
        {
            var db = condition?.transaction?.Connection ?? GetDbConnection(ConnStr);

            if (condition.TableType == null)
                condition.WithTable<TClass>();

            var sql = SqlStringEngine.GenerateUpdateSql(entity, condition);

            return db.Execute(sql, entity, condition?.transaction);
        }
    }
}
