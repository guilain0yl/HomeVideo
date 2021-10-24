using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Drapper.Core
{
    public interface ITransaction
    {
        IDbTransaction BeginTransaction(string conn = null);

        void CommitTransaction(IDbTransaction dbTransaction);

        void RollbackTransaction(IDbTransaction dbTransaction);
    }
}
