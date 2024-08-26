using CryptoTest.Domain.Models;

namespace CryptoTest.Persistence.Interfaces;

/// <summary>
/// Interface for the MessageRepository
/// Methods:
///     GetAll() - Get all messages
///     GetById(int id) - Get a message by id
///     GetAllMessagesSince(DateTime since) - Get all messages since a certain date
///     CreateMessage(Message message) - Create a new message
/// </summary>
public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetAll();
    Task<Message> GetById(int id);
    Task<IEnumerable<Message>> GetAllMessagesSince(DateTime since);
    Task CreateMessage(Message message);
}