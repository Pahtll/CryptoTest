using CryptoTest.Domain.Models;

namespace CryptoTest.Application.Services;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetAll();
    Task<Message> GetById(int id);
    Task<IEnumerable<Message>> GetAllMessagesSince(DateTime since);
    Task Create(Message message);
}