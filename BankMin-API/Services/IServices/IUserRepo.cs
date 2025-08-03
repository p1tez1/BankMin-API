using BankMin_API.Infrastructure.Entity;

namespace BankMin_API.Services.IServices
{
    public interface IUserRepo
    {
        Task CreateUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}
