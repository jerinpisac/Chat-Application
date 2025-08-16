namespace backend.Entities;

public class FriendRequest
{
    public int UserId1 { get; set; }
    public Users? User { get; set; }
    public int UserId2 { get; set; }
}
