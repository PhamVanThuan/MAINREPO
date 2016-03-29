using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SAHL.Core.Data.Configuration;

namespace SAHL.Core.Data.Context
{
    public class DefaultDbConnectionProvider : IDbConnectionProvider
    {
        private readonly IDbConfigurationProvider configurationProvider;
        private readonly object lockObject = new object();

        private readonly ConcurrentDictionary<string, List<Tuple<string, int, SqlConnection>>> registeredContexts;
        private readonly ConcurrentStack<string> unitOfWorkQueue;

        public DefaultDbConnectionProvider(IDbConfigurationProvider configurationProvider)
        {
            unitOfWorkQueue = new ConcurrentStack<string>();
            registeredContexts = new ConcurrentDictionary<string, List<Tuple<string, int, SqlConnection>>>();
            this.configurationProvider = configurationProvider;
        }

        protected int NoUnitOfWorkContexts
        {
            get { return unitOfWorkQueue.Count; }
        }

        protected int NoRegisteredContexts
        {
            get { return registeredContexts.Count; }
        }

        public bool HasUnitOfWorkContexts
        {
            get { return unitOfWorkQueue.Count > 0; }
        }

        public bool HasRegisteredContexts
        {
            get { return registeredContexts.Count > 0; }
        }

        public void RegisterUnitOfWorkContext(string uowContextName)
        {
            if (string.IsNullOrEmpty(uowContextName))
            {
                throw new ArgumentNullException(uowContextName, "uowContextName");
            }

            lock (lockObject)
            {
                var existingItem = unitOfWorkQueue.FirstOrDefault(x => x == uowContextName);
                if (existingItem != null)
                {
                    return;
                }

                unitOfWorkQueue.Push(uowContextName);
            }
        }

        public void UnregisterUnitOfWorkContext(string uowContextName)
        {
            lock (lockObject)
            {
                if (unitOfWorkQueue.Count == 0)
                {
                    return;
                }

                string removedItem;
                unitOfWorkQueue.TryPop(out removedItem);

                if (unitOfWorkQueue.Count != 0)
                {
                    return;
                }

                // close all registered contexts
                foreach (var kvp in registeredContexts)
                {
                    foreach (var tuplekvp in kvp.Value)
                    {
                        var connection = tuplekvp.Item3;
                        if (connection == null)
                        {
                            continue;
                        }
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Dispose();
                        connection = null;
                    }
                }
                registeredContexts.Clear();
            }
        }

        public IDbConnection RegisterContext(string connectionContextName)
        {
            if (string.IsNullOrEmpty(connectionContextName))
            {
                throw new ArgumentNullException(connectionContextName, "connectionContextName");
            }

            lock (this.lockObject)
            {
                var contextItem = this.GetConnectionContextItem(connectionContextName);
                if (contextItem == null)
                {
                    contextItem = this.CreateNewConnectionContextItem(connectionContextName);
                    var currentTransaction = System.Transactions.Transaction.Current;
                    if (currentTransaction != null)
                    {
                        contextItem.Item3.EnlistTransaction(currentTransaction);
                    }
                }
                else
                {
                    var currentTransaction = System.Transactions.Transaction.Current;
                    if (currentTransaction != null)
                    {
                        contextItem.Item3.EnlistTransaction(currentTransaction);
                    }
                    var newContextItem = new Tuple<string, int, SqlConnection>(contextItem.Item1, contextItem.Item2 + 1,
                        contextItem.Item3);
                    registeredContexts[connectionContextName].Remove(contextItem);
                    registeredContexts[connectionContextName].Add(newContextItem);
                }

                return this.GetConnectionForContext(connectionContextName);
            }
        }

        public void UnRegisterContext(string connectionContextName)
        {
            if (string.IsNullOrEmpty(connectionContextName))
            {
                throw new ArgumentNullException(connectionContextName, "connectionContextName");
            }

            lock (this.lockObject)
            {
                var contextItem = this.GetConnectionContextItem(connectionContextName);
                if (contextItem == null)
                {
                    return;
                }

                var newContextItem = new Tuple<string, int, SqlConnection>(contextItem.Item1,
                    contextItem.Item2 > 0 ? contextItem.Item2 - 1 : 0, contextItem.Item3);
                registeredContexts[connectionContextName].Remove(contextItem);

                if ((unitOfWorkQueue.Count == 0) && (newContextItem.Item2 == 0))
                {
                    var connection = contextItem.Item3;

                    if (connection != null)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Dispose();
                        connection = null;
                    }
                }
                else
                {
                    // add back the tuple with a changed count when in a UOW
                    registeredContexts[connectionContextName].Add(newContextItem);
                }

                if (registeredContexts[connectionContextName].Count != 0)
                {
                    return;
                }
                List<Tuple<string, int, SqlConnection>> removedContextList;
                registeredContexts.TryRemove(connectionContextName, out removedContextList);
            }
        }

        public IDbConnection GetConnectionForContext(string connectionContextName)
        {
            if (string.IsNullOrEmpty(connectionContextName))
            {
                throw new ArgumentNullException(connectionContextName, "connectionContextName");
            }

            lock (this.lockObject)
            {
                if (!registeredContexts.ContainsKey(connectionContextName))
                {
                    throw new Exception(string.Format("No DbContext has been registered for context named {0}.",
                        connectionContextName));
                }
                var contextItem = this.GetConnectionContextItem(connectionContextName);
                if (contextItem == null)
                {
                    return null;
                }

                var connection = contextItem.Item3;
                this.ConnectionEnlistTransaction(connection);
                return connection;
            }
        }

        protected bool IsUnitOfWorkContextInQueue(string uowContextName)
        {
            if (string.IsNullOrWhiteSpace(uowContextName))
            {
                return false;
            }

            lock (lockObject)
            {
                var existingItem = unitOfWorkQueue.FirstOrDefault(x => x == uowContextName);
                return existingItem != null;
            }
        }

        private SqlConnection CreateNewDbConnection(string connectionContextName)
        {
            var connectionString = configurationProvider.GetConnectionStringForNamedRole(connectionContextName);
            var connection = new SqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        private Tuple<string, int, SqlConnection> GetConnectionContextItem(string connectionContextName)
        {
            if (!registeredContexts.ContainsKey(connectionContextName))
            {
                return null;
            }

            var unitOfWorkItem = this.GetParentUnitOfWorkContext();
            var contextItemList = registeredContexts[connectionContextName];
            var contextItem = contextItemList.FirstOrDefault(x => x.Item1 == unitOfWorkItem);

            return contextItem;
        }

        private string GetParentUnitOfWorkContext()
        {
            string currentUnitOfWorkContext = unitOfWorkQueue.LastOrDefault();
            return currentUnitOfWorkContext;
        }

        private Tuple<string, int, SqlConnection> CreateNewConnectionContextItem(string connectionContextName)
        {
            var unitOfWorkItem = this.GetParentUnitOfWorkContext();
            var connection = this.CreateNewDbConnection(connectionContextName);
            var newContextItem = new Tuple<string, int, SqlConnection>(unitOfWorkItem, 1, connection);

            if (!registeredContexts.ContainsKey(connectionContextName))
            {
                var newContextList = new List<Tuple<string, int, SqlConnection>>();
                registeredContexts.TryAdd(connectionContextName, newContextList);
            }

            registeredContexts[connectionContextName].Add(newContextItem);

            return newContextItem;
        }

        private void ConnectionEnlistTransaction(SqlConnection connection)
        {
            var currentTransaction = System.Transactions.Transaction.Current;
            if (currentTransaction == null)
            {
                return;
            }

            connection.EnlistTransaction(currentTransaction);
        }
    }
}