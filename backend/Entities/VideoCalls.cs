namespace backend.Entities;

public class VideoCalls
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public Users? Users { get; set; }
    public int GroupId { get; set; }
    public Groups? Groups { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public required string Status { get; set; }
}