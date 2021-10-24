using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Drapper.Core.SqlStringHelper;
using System.Linq;

namespace Drapper.Core.DBOperation
{
    public partial class SqlOperation<TClass>
    {
        #region Insert

        protected int Insert(TClass entity, IDbTransaction dbTransaction = null)
            => InsertImpl(null, entity, dbTransaction);

        protected int Insert(string tableName, TClass entity, IDbTransaction dbTransaction = null)
            => InsertImpl(tableName, entity, dbTransaction);

        protected int Insert<TIn>(TIn entity, IDbTransaction dbTransaction = null)
            => InsertImpl(null, entity, dbTransaction);

        protected int Insert<TIn>(string tableName, TIn entity, IDbTransaction dbTransaction = null)
            => InsertImpl(tableName, entity, dbTransaction);

        private int InsertImpl<TIn>(string tableName, TIn entity, IDbTransaction dbTransaction)
        {
            var db = dbTransaction?.Connection ?? GetDbConnection(ConnStr);

            var sql = SqlStringEngine.GenerateInsertSql(entity, tableName, out bool autoInc);

            return autoInc
                ? db.ExecuteScalar<int>(sql, entity, dbTransaction)
                : db.Execute(sql, entity, dbTransaction);
        }

        #endregion

        #region BatchInsert

        protected int BatchInsert(IEnumerable<TClass> entities, IDbTransaction dbTransaction = null)
            => BatchInsertImpl(null, entities, dbTransaction);

        protected int BatchInsert(string tableName, IEnumerable<TClass> entities, IDbTransaction dbTransaction = null)
            => BatchInsertImpl(tableName, entities, dbTransaction);

        protected int BatchInsert<TIn>(IEnumerable<TIn> entities, IDbTransaction dbTransaction = null)
            => BatchInsertImpl(null, entities, dbTransaction);

        protected int BatchInsert<TIn>(string tableName, IEnumerable<TIn> entities, IDbTransaction dbTransaction = null)
            => BatchInsertImpl(tableName, entities, dbTransaction);

        private int BatchInsertImpl<TIn>(string tableName, IEnumerable<TIn> entities, IDbTransaction dbTransaction)
        {
            if (entities.Count() < 1) return -1;

            var db = dbTransaction?.Connection ?? GetDbConnection(ConnStr);

            var sql = SqlStringEngine.GenerateBatchInsertSql(entities, tableName);

            return db.Execute(sql, entities, dbTransaction);
        }

        #endregion

        #region InsertIfNoExist

        protected int InsertIfNotExist(TClass entity, Condition condition = null)
            => InsertIfNotExistImpl(entity, condition);

        protected int InsertIfNotExist<TIn>(TIn entity, Condition condition = null)
            => InsertIfNotExistImpl(entity, condition);

        private int InsertIfNotExistImpl<TIn>(TIn entity, Condition condition)
        {
            var db = condition?.transaction?.Connection ?? GetDbConnection(ConnStr);

            var sql = SqlStringEngine.GenerateInsertIfNotExistSql(entity, out var autoInc, condition);

            return autoInc
                ? db.ExecuteScalar<int>(sql, entity, condition?.transaction)
                : db.Execute(sql, entity, condition?.transaction);
        }

        #endregion
    }
}
