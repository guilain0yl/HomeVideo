using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Drapper.Core.DBOperation
{
    public abstract partial class SqlOperation<TClass>
    {
        protected abstract string ConnStr { get; }

        protected IDbConnection GetDbConnection(string conn) => DatabaseFactory.GetDbConnection(conn) ?? throw new ArgumentNullException("The IDbConnection instance is null.");

        private IDbConnection GetTranDbConnection(string conn) => DatabaseFactory.GetTranDbConnection(conn) ?? throw new ArgumentNullException("The transaction IDbConnection instance is null.");
    }
}
