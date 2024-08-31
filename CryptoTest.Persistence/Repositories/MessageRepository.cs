using System.Data;
using CryptoTest.Domain.Models;
using CryptoTest.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace CryptoTest.Persistence.Repositories;

public class MessageRepository(
    SqlDatabase sqlDatabase,
    ILogger<MessageRepository> logger
    ) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetAll()
    {
        await using var connection = sqlDatabase.GetConnection();
        await connection.OpenAsync();
        
        if(connection.State != ConnectionState.Open)
        {
            logger.LogError("Connection is not open");
            throw new Exception("Connection is not open");
        }
        logger.LogInformation("Connection to the database established");
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id SERIAL PRIMARY KEY, Text VARCHAR(128), SentAt TIMESTAMP)";

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
            logger.LogCritical("Connection is not open");
            throw new Exception("Connection is not open");
        }
        logger.LogInformation("Connection to the database established, Fetching message with id {id}", id);
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id SERIAL PRIMARY KEY, Text VARCHAR(128), SentAt TIMESTAMP)";
        
        await createTableCommand.ExecuteNonQueryAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Messages WHERE Id = @Id";
        command.Parameters.Add(
            new NpgsqlParameter("@Id", NpgsqlDbType.Integer) { Value = id });

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
    
    public async Task<IEnumerable<Message>> GetAllMessagesSince(DateTime since, DateTime until)
    {
        await using var connection = sqlDatabase.GetConnection();
        await connection.OpenAsync();
        
        if(connection.State != ConnectionState.Open)
        {
            logger.LogCritical("Connection is not open");
            throw new Exception("Connection is not open");
        }
        
        logger.LogInformation("Connection to the database established, Fetching messages since {since}", since);
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id SERIAL PRIMARY KEY, Text VARCHAR(128), SentAt TIMESTAMP)";
        
        await createTableCommand.ExecuteNonQueryAsync();
        
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Messages WHERE SentAt >= @Since AND SentAt <= @Until";
        command.Parameters.Add(
            new NpgsqlParameter("@Since", NpgsqlDbType.Timestamp) { Value = since });
        command.Parameters.Add(
            new NpgsqlParameter("@Until", NpgsqlDbType.Timestamp) { Value = until });
        
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

    public async Task<int> CreateMessage(Message message)
    {
        await using var connection = sqlDatabase.GetConnection();
        await connection.OpenAsync();
        
        if(connection.State != ConnectionState.Open)
        {
            throw new Exception("Connection is not open");
        }
        
        var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = 
            "CREATE TABLE IF NOT EXISTS Messages (Id SERIAL PRIMARY KEY, Text VARCHAR(128), SentAt TIMESTAMP)";
        
        await createTableCommand.ExecuteNonQueryAsync();

        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Messages (Text, SentAt) VALUES (@Text, @SentAt)";
        command.Parameters.Add(
            new NpgsqlParameter("@Text", NpgsqlDbType.Varchar) { Value = message.Text });
        command.Parameters.Add(
            new NpgsqlParameter("@SentAt", NpgsqlDbType.Timestamp) { Value = message.SentAt });

        await command.ExecuteNonQueryAsync();
        
        logger.LogInformation("Message created");

        var lastInsertedIdCommand = connection.CreateCommand();
        lastInsertedIdCommand.CommandText = "SELECT lastval()";
        var lastInsertedIdLong = (long) (await lastInsertedIdCommand.ExecuteScalarAsync()
                                        ?? throw new Exception("Last inserted id is null"));
        var lastInsertedId = Convert.ToInt32(lastInsertedIdLong);
        
        return lastInsertedId;
    }
}
