using MPService.Application.Auth;
using MPService.Application.Auth.DTOs;
using MPService.Application.Users.DTOs;
using MPService.Common;
using MPService.Domain.Users;

namespace MPService.Application.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserAppService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Result<UserDto>.Failure("User already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            var result = await _userRepository.AddAsync(user);
            if (!result)
            {
                return Result<UserDto>.Failure("User registration failed.");
            }

            existingUser = await _userRepository.GetByEmailAsync(request.Email);

            var userDto = new UserDto
            {
                Id = existingUser!.Id.ToString(),
                Username = user.Username,
                Email = user.Email
            };

            return Result<UserDto>.Success(userDto);
        }

        public async Task<AuthResultDto> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !user.VerifyPassword(request.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _jwtService.GenerateToken(user);
            return new AuthResultDto { Token = token, Email = user.Email, Username = user.Username };
        }
    }
}
