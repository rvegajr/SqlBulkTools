using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlBulkTools
{
    internal interface ITransaction
    {
        int CommitTransaction(string connectionName = null, SqlCredential credentials = null, SqlConnection connection = null);
        Task<int> CommitTransactionAsync(string connectionName = null, SqlCredential credentials = null, SqlConnection connection = null);
    }
}
