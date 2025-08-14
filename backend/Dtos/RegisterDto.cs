namespace backend.Dtos;

public record class RegisterDto
(
    string FullName,
    string Email,
    string Password ,
    string ProfilePic, 
    string Bio,
    string Language, 
    string Status,
    DateTime JoinedAt 
);