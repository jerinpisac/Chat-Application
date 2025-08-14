namespace backend.Entities;

public class GroupMessages
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int UserId { get; set; }
    public string? Content { get; set; }
    public string? React { get; set; }
    public required string Type { get; set; }
    public DateTime SendAt { get; set; }
    public string? MediaUri { get; set; }
    public int ReplyToId { get; set; }
    public Groups? Groups { get; set; }
    public Users? Users { get; set; }
}