using CryptoTest.Domain.Models;

namespace CryptoTest.Application.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetAll();
    Task<Message> GetById(int id);
    Task<IEnumerable<Message>> GetAllMessagesSince(DateTime since, DateTime until);
    Task<int> Create(Message message);
}