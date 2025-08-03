using BankMin_API.Features.Auth.Login;
using BankMin_API.Features.Auth.SignUp;
using MediatR;

namespace BankMin_API.Features.Auth
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/auth").WithTags("Auth");

            group.MapPost("/signup", async (SignUpCommand cmd, ISender sender) =>
            await sender.Send(cmd));

            group.MapPost("/login", async(LoginCommand cmd, ISender sender) =>
            await sender.Send(cmd));
        }
    }
}
