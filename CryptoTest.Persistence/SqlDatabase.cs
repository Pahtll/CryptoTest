using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;


namespace CryptoTest.Persistence;

public class SqlDatabase(string? connectionString)
{
    private readonly string _connectionString = connectionString 
                                                ?? throw new ArgumentNullException(nameof(connectionString));
    
    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}