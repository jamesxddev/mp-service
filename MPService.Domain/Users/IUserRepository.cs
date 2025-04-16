namespace MPService.Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> AddAsync(User user);
    }
}
