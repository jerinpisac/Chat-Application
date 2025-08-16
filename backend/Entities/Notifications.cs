namespace backend.Entities;

public class Notifications
{
    public int Id { get; set; }
    public int UserId1 { get; set; }
    public Users? Users { get; set; }
    public required string Type { get; set; }
    public string? Content { get; set; }
    public DateTime SendAt { get; set; }
    public int? GroupId { get; set; }
    public int? UserId2 { get; set; }
    public Groups? Groups { get; set; }
    public bool IsSeen { get; set; }
}
