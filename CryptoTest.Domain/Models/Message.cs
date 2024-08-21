namespace CryptoTest.Domain.Models;

public class Message
{
    public int Id { get; set; }
    
    public string Text { get; set; } = string.Empty;
    
    public DateTime SentAt { get; set; } = DateTime.Now;
}