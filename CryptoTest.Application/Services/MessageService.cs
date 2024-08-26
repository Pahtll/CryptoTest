using CryptoTest.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace CryptoTest.Application.Services;

public class MessageService(
    IMessageRepository messageRepository,
    ILogger<MessageService> logger
    )
{
    
}