using System.Data;
using CryptoTest.Domain.Models;
using CryptoTest.Persistence;
using Microsoft.Data.SqlClient;

namespace Persistence.Repositories;

public class MessageRepository(SqlDatabase sqlDatabase)
{
    public async Task<IEnumerable<Message>> GetAll()
    {
        await using var connection = sqlDatabase.GetConnection();
        await connection.OpenAsync();
        
        if(connection.State != ConnectionState.Open)
        {
            throw new Exception("Connection is not open");
        }
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id INT PRIMARY KEY IDENTITY, Text NVARCHAR(128), SentAt DATETIME)";

        await createTableCommand.ExecuteNonQueryAsync();
        
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Messages";

        var messages = new List<Message>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            messages.Add(new Message
            {
                Id = reader.GetInt32(0),
                Text = reader.GetString(1),
                SentAt = reader.GetDateTime(2)
            });
        }

        return messages;
    }

    public async Task<Message> GetById(int id)
    {
        await using var connection = sqlDatabase.GetConnection();
        await connection.OpenAsync();
        
        if(connection.State != ConnectionState.Open)
        {
            throw new Exception("Connection is not open");
        }
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id INT PRIMARY KEY IDENTITY, Text NVARCHAR(128), SentAt DATETIME)";
        
        await createTableCommand.ExecuteNonQueryAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Messages WHERE Id = @Id";
        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Message
            {
                Id = reader.GetInt32(0),
                Text = reader.GetString(1),
                SentAt = reader.GetDateTime(2)
            };
        }

        throw new ArgumentException("Message not found");
    }
    
    public async Task<IEnumerable<Message>> GetAllMessagesSince(DateTime since)
    {
        await using var connection = sqlDatabase.GetConnection();
        await connection.OpenAsync();
        
        if(connection.State != ConnectionState.Open)
        {
            throw new Exception("Connection is not open");
        }
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id INT PRIMARY KEY IDENTITY, Text NVARCHAR(128), SentAt DATETIME)";
        
        await createTableCommand.ExecuteNonQueryAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Messages WHERE CreatedAt >= @Since";
        command.Parameters.Add(new SqlParameter("@Since", SqlDbType.DateTime) { Value = since });

        var messages = new List<Message>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            messages.Add(new Message
            {
                Id = reader.GetInt32(0),
                Text = reader.GetString(1),
                SentAt = reader.GetDateTime(2)
            });
        }

        return messages;
    }

    public async Task CreateMessage(Message message)
    {
        await using var connection = sqlDatabase.GetConnection();
        await connection.OpenAsync();
        
        if(connection.State != ConnectionState.Open)
        {
            throw new Exception("Connection is not open");
        }
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id INT PRIMARY KEY IDENTITY, Text NVARCHAR(128), SentAt DATETIME)";
        
        await createTableCommand.ExecuteNonQueryAsync();

        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Messages (Content, CreatedAt) VALUES (@Text, @SentAt)";
        command.Parameters.Add(new SqlParameter("@Content", SqlDbType.NVarChar) { Value = message.Text });
        command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = message.SentAt });

        await command.ExecuteNonQueryAsync();
    }
}