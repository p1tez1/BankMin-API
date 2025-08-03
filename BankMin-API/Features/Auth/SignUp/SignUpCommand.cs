using MediatR;

namespace BankMin_API.Features.Auth.SignUp
{
    public record class SignUpCommand : IRequest<IResult>
    {
        public string Name { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
