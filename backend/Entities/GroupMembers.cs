namespace backend.Entities;

public class GroupMembers
{
    public int GroupId { get; set; }
    public int UserId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string Role { get; set; } = "member";
    public Groups? Groups { get; set; }
    public Users? Users { get; set; }
}
