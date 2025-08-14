namespace backend.Entities;

public class GroupMessagesSeen
{
    public int GroupMessageId { get; set; }
    public GroupMessages? GroupMessages { get; set; }
    public int UserId { get; set; }
    public Users? Users { get; set; }
    public DateTime SeenAt { get; set; }
}
