using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlBulkTools
{
    /// <summary>
    /// 
    /// </summary>
    public class BulkOperations : IBulkOperations
    {
        private ITransaction _transaction;
        private const string SourceAlias = "Source";
        private const string TargetAlias = "Target";  

        internal void SetBulkExt(ITransaction transaction)
        {
            _transaction = transaction;
        }

        /// <summary>
        /// Commits a transaction to database. A valid setup must exist for operation to be 
        /// successful. Notes: (1) The connectionName parameter is a name that you provide to 
        /// uniquely identify a connection string so that it can be retrieved at run time.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="connectionName"></param>
        /// <param name="credentials"></param>
        public int CommitTransaction(string connectionName, SqlCredential credentials = null)
        {
            if (connectionName == null)
                throw new ArgumentNullException(nameof(connectionName) + " not given");

            if (_transaction == null)
                throw new InvalidOperationException("No setup found. Use the Setup method to build a new setup then try again.");
            

            return _transaction.CommitTransaction(connectionName, credentials);
        }

        /// <summary>
        /// Commits a transaction to database. A valid setup must exist for operation to be 
        /// successful. Notes: (1) The connectionName parameter is a name that you provide to 
        /// uniquely identify a connection string so that it can be retrieved at run time.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<int> CommitTransactionAsync(string connectionString, SqlCredential credentials = null)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString) + " not given");

            if (_transaction == null)
                throw new InvalidOperationException("No setup found. Use the Setup method to build a new setup then try again.");

            return await _transaction.CommitTransactionAsync(connectionString, credentials);
        }


        /// <summary>
        /// Commits a transaction to database. A valid setup must exist for operation to be 
        /// successful. 
        /// </summary>
        /// <param name="connection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public int CommitTransaction(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (_transaction == null)
                throw new InvalidOperationException("No setup found. Use the Setup method to build a new setup then try again.");

            return _transaction.CommitTransaction(connection : connection);

        }


        /// <summary>
        /// Commits a transaction to database. A valid setup must exist for operation to be 
        /// successful. 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<int> CommitTransactionAsync(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (_transaction == null)
                throw new InvalidOperationException("No setup found. Use the Setup method to build a new setup then try again.");

            return await _transaction.CommitTransactionAsync(connection : connection);
        }

        /// <summary>
        /// Each transaction requires a valid setup. Examples available at: https://github.com/gtaylor44/SqlBulkTools 
        /// </summary>
        /// <typeparam name="T">The type of collection to be used.</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public CollectionSelect<T> Setup<T>(Func<Setup<T>, CollectionSelect<T>> list)
        {
            CollectionSelect<T> tableSelect = list(new Setup<T>(SourceAlias, TargetAlias, this));
            return tableSelect;
        }
    }
}