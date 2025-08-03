using BankMin_API.Features.Auth;
using BankMin_API.Infrastructure;
using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;

namespace BankMin_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IHashService _hashService;
        private readonly IRoleRepo _roleRepo;
        private readonly IJWTGenerator _jwtGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;
        private readonly BankDbContext _dBcontext;
        public UserService(IUserRepo userRepo, IRoleRepo roleRepo, IJWTGenerator jWTGenerator, IHttpContextAccessor httpContext, IHashService hashService, ILogger<UserService> logger, BankDbContext dBcontext)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _jwtGenerator = jWTGenerator;
            _httpContextAccessor = httpContext;
            _hashService = hashService;
            _dBcontext = dBcontext;
            _logger = logger;
        }
        public async Task SignUpUser(User user)
        {
            using var transaction = await _dBcontext.Database.BeginTransactionAsync();
            try
            {
                await _userRepo.CreateUserAsync(user);

                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                };
                await _roleRepo.ClaimRoleAsync(userRole);

                var token = await _jwtGenerator.GenerateToken(user);

                _httpContextAccessor.HttpContext.Response.Cookies
                    .Append("access_token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    });
                await transaction.CommitAsync();
                _logger.LogInformation("Create new customer with email: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("Sign up failed cause: {ex}", ex);
                throw;
            }
        }
        public async Task LoginUser(string email, string password)
        {
            using var transaction = await _dBcontext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userRepo.GetUserByEmailAsync(email);
                if (user == null)
                    throw new UnauthorizedAccessException("User not found");

                if (!_hashService.Verify(password, user.Password))
                    throw new UnauthorizedAccessException("Incorrect credentials");

                var token = await _jwtGenerator.GenerateToken(user);

                _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });

                await transaction.CommitAsync();
                _logger.LogInformation("Customer {email} logged in", user.Email);
            }
            catch (UnauthorizedAccessException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected login error");
                throw;
            }
        }

    }
}

