using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;


namespace CryptoTest.Persistence;

public class SqlDatabase(IConfiguration configuration)
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}