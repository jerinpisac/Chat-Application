namespace backend.Dtos;

public record class UserDto
(
    int Id,
    string FullName,
    string Email,
    string? ProfilePic, 
    string? Bio,
    string Language, 
    string? Status,
    string JoinedAt,
    bool SentRequest 
);