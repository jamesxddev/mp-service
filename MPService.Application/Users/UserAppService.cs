using MPService.Application.Users.DTOs;
using MPService.Common;
using MPService.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPService.Application.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
