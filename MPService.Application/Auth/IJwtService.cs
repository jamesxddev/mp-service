using MPService.Domain.Users;

namespace MPService.Application.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();


    }
}
