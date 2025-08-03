using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace BankMin_API.Infrastructure.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly BankDbContext _context;
        public UserRepo(BankDbContext context)
        {
            _context = context;
        }
        public async Task CreateUserAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;   
        }
    }
}
