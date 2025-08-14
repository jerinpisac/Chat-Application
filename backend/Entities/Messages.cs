namespace backend.Entities;

public class Messages
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public DateTime SendAt { get; set; }
    public string? Content { get; set; }
    public string? React { get; set; }
    public required string Type { get; set; }
    public bool IsSeen { get; set; } = false;
    public string? MediaUri { get; set; }
    public int ReplyToId { get; set; }
    public Users? Users { get; set; }
}
