using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Drapper.Core.DBOperation
{
    public partial class SqlOperation<TClass> : ITransaction
    {
        public virtual IDbTransaction BeginTransaction(string conn = null)
        {
            IDbConnection db = null;

            try
            {
                if (string.IsNullOrEmpty(conn))
                    conn = ConnStr;
                db = GetTranDbConnection(conn);
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                return db.BeginTransaction();
            }
            catch (Exception ex)
            {
                db.Dispose();
                throw ex;
            }
        }

        public virtual void CommitTransaction(IDbTransaction dbTransaction)
        {
            var conn = dbTransaction.Connection;
            try
            {
                dbTransaction.Commit();
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }

        public virtual void RollbackTransaction(IDbTransaction dbTransaction)
        {
            var conn = dbTransaction.Connection;
            try
            {
                dbTransaction.Rollback();
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }
    }
}
