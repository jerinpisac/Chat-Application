namespace backend.Dtos;

public record class NotificationsDto
(
    int UserId1,
    string FullName,
    string ProfilePic,
    string Type,
    string Content,
    string SendAt,
    string GroupName
);