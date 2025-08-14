namespace backend.Entities;

public class Users
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? ProfilePic { get; set; }
    public string Bio { get; set; } = "";
    public required string Language { get; set; }
    public string Status { get; set; } = "online";
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
