using MPService.Application.Auth.DTOs;
using MPService.Application.Users.DTOs;
using MPService.Common;
using MPService.Domain.Users;

namespace MPService.Application.Users
{
    public interface IUserAppService
    {
        Task<Result<UserDto>> RegisterAsync(RegisterRequest request);
        Task<AuthResultDto> LoginAsync(LoginRequest request);
        Task<Result<string>> UpdatePasswordAsync(string username, UpdatePasswordRequest request);
    }
}
