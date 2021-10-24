using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Drapper.Core
{
    internal class DatabaseFactory
    {
        DatabaseFactory()
        {
#if !NETFRAMEWORK
            DbProviderFactories.RegisterFactory(SqlServer, SqlClientFactory.Instance);
#endif
            _factory = DbProviderFactories.GetFactory(SqlServer);
        }

        /// <summary>
        /// get normal connection,if we can get it from cache,we will get it
        /// </summary>
        /// <param name="conn">connection string</param>
        /// <returns></returns>
        internal static IDbConnection GetDbConnection(string conn) => Instance.CreateDbConnection(conn);

        /// <summary>
        /// get the connection for transaction
        /// </summary>
        /// <param name="conn">connection string</param>
        /// <returns></returns>
        internal static IDbConnection GetTranDbConnection(string conn) => Instance.CreateDbConnection(conn);

        private IDbConnection GetDbConnectionWithCache(string conn)
        {
            if (_dbCache.TryGetValue(conn, out var dbConnections))
            {
                lock (_lock)
                {
                    var connection = dbConnections.Where(x => x.State == ConnectionState.Closed)?.FirstOrDefault();

                    if (connection == null)
                    {
                        connection = CreateDbConnection(conn);
                        dbConnections.Add(connection);
                    }
                    else
                    {
                        if (string.Compare(connection.ConnectionString, conn) != 0)
                            connection.ConnectionString = conn;
                    }

                    return connection;
                }
            }
            else
            {
                if (_dbCache.TryAdd(conn, new List<IDbConnection>()))
                {
                    return GetDbConnectionWithCache(conn);
                }

                return null;
            }
        }

        private IDbConnection CreateDbConnection(string conn)
        {
            if (string.IsNullOrEmpty(conn)) throw new Exception("SQL Connection string is empty.");

            var _sharedConnection = _factory.CreateConnection();

            if (_sharedConnection == null) throw new Exception("SQL Connection failed to configure.");

            if (string.Compare(_sharedConnection.ConnectionString, conn) != 0)
                _sharedConnection.ConnectionString = conn;

            if (_sharedConnection.State == ConnectionState.Broken || _sharedConnection.State == ConnectionState.Open)
            {
                _sharedConnection.Close();
            }

            return _sharedConnection;
        }

        private readonly ConcurrentDictionary<string, IList<IDbConnection>> _dbCache = new ConcurrentDictionary<string, IList<IDbConnection>>();
        private static readonly DatabaseFactory Instance = new DatabaseFactory();
        private readonly DbProviderFactory _factory = null;
        private const string SqlServer = "System.Data.SqlClient";

        private readonly object _lock = new object();
    }
}
