using CryptoTest.Application.Interfaces;
using CryptoTest.Domain.Models;
using CryptoTest.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace CryptoTest.Application.Services;

public class MessageService(
    IMessageRepository messageRepository,
    ILogger<MessageService> logger
    ) : IMessageService
{
    private const int MessageMaxLenght = 128;

    public async Task<IEnumerable<Message>> GetAll()
    {
        try
        {
            return await messageRepository.GetAll();
        }
        catch (ArgumentException e)
        {
            logger.LogError("Messages are not found");
            throw new ArgumentException("Messages are not found");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while fetching all messages");
            throw new Exception("An error occurred while fetching all messages");
        }
    }

    public async Task<Message> GetById(int id)
    {
        try
        {
            return await messageRepository.GetById(id);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e, "Message doesn't exists");
            throw new ArgumentException("Message doesn't exists");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while fetching message with id {id}", id);
            throw new Exception($"An error occurred while fetching message with id {id}");
        }
    }

    public async Task<IEnumerable<Message>> GetAllMessagesSince(DateTime since, DateTime until)
    {
        if (since == default)
        {
            logger.LogError("Since is default");
            throw new ArgumentException("Since is default");
        }
        if (until == default)
        {
            logger.LogError("Until is default");
            throw new ArgumentException("Until is default");
        }
        if (since > until)
        {
            logger.LogError("Since is greater than until");
            throw new ArgumentException("Since is greater than until");
        }
        if (since == until)
        {
            logger.LogError("Since is equal to until");
            throw new ArgumentException("Since is equal to until");
        }
        if (until > DateTime.Now)
        {
            logger.LogError("Until is greater than now");
            throw new ArgumentException("Until is greater than now");
        }

        try
        {
            return await messageRepository.GetAllMessagesSince(since, until);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e, "Messages isn't found");
            throw new ArgumentException("Messages isn't found"); 
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while fetching all messages since {since}", since);
            throw new Exception($"An error occurred while fetching all messages since {since}");
        }
    }
    
    public async Task<int> Create(Message message)
    {
        if (message == null)
        {
            logger.LogError("Message is null");
            throw new ArgumentNullException(nameof(message));
        }
        if (string.IsNullOrWhiteSpace(message.Text))
        {
            logger.LogError("Message text is null or empty");
            throw new ArgumentException("Message text is null or empty");
        }

        if (message.Text.Length > MessageMaxLenght)
        {
            logger.LogError("Message text is too long");
            throw new ArgumentException("Message text is too long");
        }
        if (message.SentAt == default)
        {
            logger.LogError("Message sent at is default");
            throw new ArgumentException("Message sent at is default");
        }

        try
        {
            return await messageRepository.CreateMessage(message);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e, "Message is not created");
            throw new ArgumentException("Message is not created");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating a message");
            throw new Exception("An error occurred while creating a message");
        }
    }
}