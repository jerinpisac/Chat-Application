using backend.Entities;

namespace backend.Services;

public interface ITokenService
{
    string CreateToken(Users user);
}