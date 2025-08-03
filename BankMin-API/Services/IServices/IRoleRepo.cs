using BankMin_API.Infrastructure.Entity;

namespace BankMin_API.Services.IServices
{
    public interface IRoleRepo
    {
        Task ClaimRoleAsync(UserRole claim);
        Task<string> GetRoleByUserIdAsync(Guid userId);

    }
}
