using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;


namespace CryptoTest.Persistence;

public class SqlDatabase(IConfiguration configuration)
{
    private readonly string? _connection = configuration.GetConnectionString("DefaultConnection");

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connection);
    }
}