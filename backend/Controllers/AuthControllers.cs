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
                findUser.JoinedAt.ToString(),
                false
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
            var result = await dbContext.FriendRequest.FirstOrDefaultAsync(t => t.UserId1 == Id.Id && t.UserId2 == user.Id);
            var res = result is not null;
            var newUser = new UserDto
            (
                user.Id,
                user.FullName,
                user.Email,
                user.ProfilePic,
                user.Bio,
                user.Language,
                user.Status,
                user.JoinedAt.ToString(),
                res
            );
            userList.Add(newUser);
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
            UserId2 = friendRequestDto.UserId2
        };

        await dbContext.Notifications.AddAsync(request);
        await dbContext.FriendRequest.AddAsync(friend);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("fetchnotifications")]
    public async Task<IActionResult> FetchNotifications([FromBody] IdDto Id)
    {
        var nots = dbContext.Notifications.Include(t => t.Users).Where(t => t.UserId2 != null && t.UserId2 == Id.Id && t.IsSeen == false);

        var ids = await dbContext.GroupMembers.Where(t => t.UserId == Id.Id).Select(t => t.GroupId).ToListAsync();

        var gnots = await dbContext.Notifications.Include(t => t.Users).Include(t => t.Groups).Where(t => t.GroupId != null && t.IsSeen == false && ids.Contains((int)t.GroupId)).ToListAsync();

        return Ok(new
        {
            Single = nots,
            Group = gnots
        });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFriendRequest()
    {
        var a = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == 6);
        dbContext.Notifications.Remove(a!);
        await dbContext.SaveChangesAsync();

        return Ok();
    }
    
}