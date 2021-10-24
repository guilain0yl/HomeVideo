using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace Drapper.Core.DBOperation
{
    public partial class SqlOperation<TClass>
    {
        public int Excute<TIn>(string sql, TIn entity, string conn = null, IDbTransaction dbTransaction = null)
            => ExcuteImpl(sql, entity, string.IsNullOrEmpty(conn) ? ConnStr : conn, dbTransaction);

        private int ExcuteImpl<TIn>(string sql, TIn entity, string conn, IDbTransaction dbTransaction)
        {
            var db = dbTransaction?.Connection ?? GetDbConnection(conn);
            return db.Execute(sql, entity, dbTransaction);
        }

        public TOut ExecuteScalar<TIn, TOut>(string sql, TIn entity, string conn = null, IDbTransaction dbTransaction = null)
            => ExecuteScalarImpl<TIn, TOut>(sql, entity, string.IsNullOrEmpty(conn) ? ConnStr : conn, dbTransaction);

        private TOut ExecuteScalarImpl<TIn, TOut>(string sql, TIn entity, string conn, IDbTransaction dbTransaction)
        {
            var db = dbTransaction?.Connection ?? GetDbConnection(conn);
            return db.ExecuteScalar<TOut>(sql, entity, dbTransaction);
        }
    }
}
