namespace backend.Entities;

public class Groups
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Description { get; set; }
    public int CreatedBy { get; set; }
    public Users? Users { get; set; }
}
