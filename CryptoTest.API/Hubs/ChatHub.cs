using System.Text.Json;
using CryptoTest.API.Interfaces;
using CryptoTest.API.Traits;
using CryptoTest.Application.Interfaces;
using CryptoTest.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace CryptoTest.API.Hubs;

public class ChatHub(
    IDistributedCache cache,
    ILogger<ChatHub> logger,
    IMessageService messageService
    ) : Hub<IChatClient>
{
    public async Task JoinChatroom(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.Chatroom);
        
        var stringifiedConnection = JsonSerializer.Serialize(connection);

        await cache.SetStringAsync(Context.ConnectionId, stringifiedConnection);
        
        await Clients
            .Group(connection.Chatroom)
            .ReceiveMessage("someone", "Someone has joined the chatroom");
    }

    public async Task SendMessage(string text)
    {
        var stringifiedConnection = await cache.GetStringAsync(Context.ConnectionId)
            ?? throw new ArgumentException("Connection not found");
        
        var connection = JsonSerializer.Deserialize<UserConnection>(stringifiedConnection)
            ?? throw new ArgumentException("Connection can not be parsed");
        
        await Clients
            .Group(connection.Chatroom)
            .ReceiveMessage(connection.Username, text);

        try
        {
            await messageService.Create(new Message
            {
                Text = text,
                SentAt = DateTime.Now
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating a message");
            throw new Exception("An error occurred while creating a message");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringifiedConnection =  await cache.GetStringAsync(Context.ConnectionId)
            ?? throw new ArgumentException("Connection not found");
        var connection = JsonSerializer.Deserialize<UserConnection>(stringifiedConnection)
            ?? throw new ArgumentException("Connection can not be parsed");

        await cache.RemoveAsync(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.Chatroom);
        
        await Clients
            .Group(connection.Chatroom)
            .ReceiveMessage("Someone", "Someone has left the chatroom");
        
        await base.OnDisconnectedAsync(exception);
    }
}