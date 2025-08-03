using BankMin_API.Infrastructure.Entity;

namespace BankMin_API.Services.IServices
{
    public interface IAccountRepo
    {
        Task CreateAccountAsync(Account account);
        Task<ICollection<Account>> GetAccountsByUserIdAsync(Guid userId);
        Task<Account> GetAccountByidAsync(Guid accountId);
    }
}
