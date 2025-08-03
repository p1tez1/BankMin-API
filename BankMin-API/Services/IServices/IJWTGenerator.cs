using BankMin_API.Infrastructure.Entity;

namespace BankMin_API.Services.IServices
{
    public interface IJWTGenerator
    {
        Task<string> GenerateToken(User user);
    }
}
