namespace CryptoTest.API.Hubs;

public interface IChatClient
{
    public Task ReceiveMessage(string username, string message);
}