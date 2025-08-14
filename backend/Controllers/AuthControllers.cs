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

        return Ok(userList);
    }
}