using BankMin_API.Infrastructure.Entity;

namespace BankMin_API.Features.Auth
{
    public interface IUserService
    {
        Task SignUpUser(User user);
        Task LoginUser(string email, string password);
    }
}
