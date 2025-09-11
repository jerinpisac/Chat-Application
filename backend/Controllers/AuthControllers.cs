using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Dtos;
using backend.Services;
using backend.contexts;
using backend.Entities;
using System.Net.WebSockets;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace Backend.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;
    private readonly ITokenService _tokenService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        dbContext = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto user)
    {
        var findUser = await dbContext.Users.FirstOrDefaultAsync(t => t.Email == user.Email);

        var message = "Invalid Credentials";

        if (findUser is null) return NotFound(message);

        if (BCrypt.Net.BCrypt.Verify(user.Password, findUser.Password))
        {
            var newUser = new UserDto(
                findUser.Id,
                findUser.FullName,
                findUser.Email,
                findUser.ProfilePic,
                findUser.Bio,
                findUser.Language,
                findUser.Status,
                findUser.JoinedAt.ToString()
            );

            var token = _tokenService.CreateToken(findUser);

            return Ok(
                new
                {
                    User = newUser,
                    token = token
                });
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto user)
    {
        if (user.FullName == "" || user.Email == "" || user.Password == "")
        {
            string Message = "Invalid Credentials";
            return Content(Message);
        }
        var result = await dbContext.Users.FirstOrDefaultAsync(id => id.Email == user.Email);
        if (result is not null)
        {
            string Message = "Email already exists";
            return Content(Message);
        }

        Users User = new()
        {
            FullName = user.FullName,
            Email = user.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Bio = user.Bio,
            Language = user.Language,
            ProfilePic = user.ProfilePic,
            Status = user.Status
        };

        await dbContext.Users.AddAsync(User);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("fetchusers")]
    public async Task<IActionResult> FetchUsersForHome([FromBody] IdDto Id)
    {
        var users = await dbContext.Users.Where(a => Id.Id != a.Id).ToListAsync();

        List<UserDto> userList = [];

        foreach (var user in users)
        {
            if (!((await dbContext.FriendRequest.FirstOrDefaultAsync(t => (t.UserId1 == user.Id && t.UserId2 == Id.Id) || (t.UserId2 == user.Id && t.UserId1 == Id.Id)) is null)
            ^ 
            (await dbContext.Friends.FirstOrDefaultAsync(t => (t.UserId1 == user.Id && t.UserId2 == Id.Id) || (t.UserId2 == user.Id && t.UserId1 == Id.Id)) is null))) {
                var newUser = new UserDto
                (
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.ProfilePic,
                    user.Bio,
                    user.Language,
                    user.Status,
                    user.JoinedAt.ToString()
                );
                userList.Add(newUser);
            }
        }

        return Ok(userList);
    }

    [HttpPost("sendrequest")]
    public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto friendRequestDto)
    {
        Notifications request = new()
        {
            UserId1 = friendRequestDto.UserId1,
            UserId2 = friendRequestDto.UserId2,
            Type = "Friend_Request",
            SendAt = DateTime.Now
        };

        FriendRequest friend = new()
        {
            UserId1 = friendRequestDto.UserId1,
            UserId2 = friendRequestDto.UserId2,
            SentAt = DateTime.Now
        };

        await dbContext.Notifications.AddAsync(request);
        await dbContext.FriendRequest.AddAsync(friend);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("declinerequest")]
    public async Task<IActionResult> DeclineRequest([FromBody] IdsDto Id)
    {
        var id1 = await dbContext.Notifications.FirstOrDefaultAsync(t => (t.UserId1 == Id.Id1 && t.UserId2 == Id.Id2) || (t.UserId1 == Id.Id2 && t.UserId2 == Id.Id1));
        dbContext.Notifications.Remove(id1!);
        await dbContext.SaveChangesAsync();

        var id2 = await dbContext.FriendRequest.FirstOrDefaultAsync(t => (t.UserId1 == Id.Id1 && t.UserId2 == Id.Id2) || (t.UserId1 == Id.Id2 && t.UserId2 == Id.Id1));
        dbContext.FriendRequest.Remove(id2!);
        await dbContext.SaveChangesAsync();

        Notifications request = new()
        {
            UserId1 = Id.Id1,
            UserId2 = Id.Id2,
            Type = "Friend_Request_Declined",
            SendAt = DateTime.Now
        };

        await dbContext.Notifications.AddAsync(request);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("removerequest")]
    public async Task<IActionResult> RemoveRequest([FromBody] IdsDto Id)
    {
        var id1 = await dbContext.Notifications.FirstOrDefaultAsync(t => (t.UserId1 == Id.Id1 && t.UserId2 == Id.Id2) || (t.UserId1 == Id.Id2 && t.UserId2 == Id.Id1));
        dbContext.Notifications.Remove(id1!);
        await dbContext.SaveChangesAsync();

        var id2 = await dbContext.FriendRequest.FirstOrDefaultAsync(t => (t.UserId1 == Id.Id1 && t.UserId2 == Id.Id2) || (t.UserId1 == Id.Id2 && t.UserId2 == Id.Id1));
        dbContext.FriendRequest.Remove(id2!);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("acceptrequest")]
    public async Task<IActionResult> AcceptRequest([FromBody] IdsDto Id)
    {
        var id1 = await dbContext.Notifications.FirstOrDefaultAsync(t => (t.UserId1 == Id.Id1 && t.UserId2 == Id.Id2) || (t.UserId1 == Id.Id2 && t.UserId2 == Id.Id1));
        dbContext.Notifications.Remove(id1!);
        await dbContext.SaveChangesAsync();

        var id2 = await dbContext.FriendRequest.FirstOrDefaultAsync(t => (t.UserId1 == Id.Id1 && t.UserId2 == Id.Id2) || (t.UserId1 == Id.Id2 && t.UserId2 == Id.Id1));
        dbContext.FriendRequest.Remove(id2!);
        await dbContext.SaveChangesAsync();

        Friends friends = new()
        {
            UserId1 = Id.Id1,
            UserId2 = Id.Id2
        };

        await dbContext.Friends.AddAsync(friends);
        await dbContext.SaveChangesAsync();

        Notifications request = new()
        {
            UserId1 = Id.Id1,
            UserId2 = Id.Id2,
            Type = "Friend_Request_Accepted",
            SendAt = DateTime.Now
        };

        await dbContext.Notifications.AddAsync(request);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("fetchrequests")]
    public async Task<IActionResult> FetchRequest([FromBody] IdDto Id)
    {
        var items = await dbContext.FriendRequest
            .Include(t => t.User)
            .Where(t => t.UserId1 == Id.Id)
            .Select(t => new { FullName = t.User!.FullName, UserId2 = t.UserId2, ProfilePic = t.User!.ProfilePic, SentAt = t.SentAt })
            .OrderByDescending(t => t.SentAt)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPatch("markasread")]
    public async Task<IActionResult> MarkAsRead([FromBody] MarkAsReadDto mar)
    {
        var id = await dbContext.Notifications
                        .FirstOrDefaultAsync(t => t.UserId1 == mar.Id1 && t.UserId2 == mar.Id2 && t.Type == mar.Type && t.IsSeen == false);

        if (id is not null)
        {
            id.IsSeen = true;
            await dbContext.SaveChangesAsync();
        }
        return Ok(id);
    }

    [HttpPost("fetchnotifications")]
    public async Task<IActionResult> FetchNotifications([FromBody] IdDto Id)
    {
        var nots = dbContext.Notifications.Include(t => t.Users)
                    .Where(t => t.UserId2 != null && t.UserId2 == Id.Id && t.IsSeen == false)
                    .OrderByDescending(t => t.SendAt);

        List<NotificationsDto> snots = [];

        foreach (var item in nots)
        {
            var x = new NotificationsDto(
                item.UserId1,
                item.Users!.FullName,
                item.Users!.ProfilePic,
                item.Type,
                item.Content ?? "",
                (item.SendAt.Date == DateTime.Today) ? "Today " + item.SendAt.TimeOfDay.ToString()[..5] : item.SendAt.ToString()[..16],
                (item.Groups == null) ? "" : item.Groups.Name
            );

            snots.Add(x);
        }

        var ids = await dbContext.GroupMembers.Where(t => t.UserId == Id.Id).Select(t => t.GroupId).ToListAsync();

        var gnots = await dbContext.Notifications.Include(t => t.Users).Include(t => t.Groups).Where(t => t.GroupId != null && t.IsSeen == false && ids.Contains((int)t.GroupId)).OrderByDescending(t => t.SendAt).ToListAsync();

        foreach (var item in gnots)
        {
            var x = new NotificationsDto(
                item.UserId1,
                item.Users!.FullName,
                item.Users!.ProfilePic,
                item.Type,
                item.Content ?? "",
                (item.SendAt.Date == DateTime.Today) ? "Today " + item.SendAt.TimeOfDay.ToString()[..5] : item.SendAt.ToString()[..16],
                (item.Groups == null) ? "" : item.Groups.Name
            );

            snots.Add(x);
        }

        return Ok(snots);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFriendRequest()
    {
        var a = await dbContext.Notifications.FirstOrDefaultAsync(x => x.UserId1 == 1);
        dbContext.Notifications.Remove(a!);
        await dbContext.SaveChangesAsync();

        return Ok();
    }
    
}