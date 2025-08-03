using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace BankMin_API.Infrastructure.Repositories
{
    public class AccountRepo : IAccountRepo
    {
        private readonly BankDbContext _context;
        public AccountRepo(BankDbContext context)
        {
            _context = context;
        }

        public async Task CreateAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account> GetAccountByidAsync(Guid accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            return account;
        }

        public async Task<ICollection<Account>> GetAccountsByUserIdAsync(Guid userId)
        {
            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .ToArrayAsync();
            return accounts;
        }

    }
}
