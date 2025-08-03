using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace BankMin_API.Infrastructure.Repositories
{
    public class RoleRepo : IRoleRepo
    {
        private readonly BankDbContext _context;

        public RoleRepo(BankDbContext context)
        {
            _context = context;
        }

        public async Task ClaimRoleAsync(UserRole claim)
        {
            await _context.UserRoles.AddAsync(claim);
            await _context.SaveChangesAsync();
        }
        public async Task<string> GetRoleByUserIdAsync(Guid userId)
        {
           var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
           var role = await _context.Roles.FindAsync(userRole.RoleId);
           return role.RoleName;
        }
    }
}
