using MediatR;

namespace BankMin_API.Features.Auth.Login
{
    public record class LoginCommand(): IRequest<IResult>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
