using BankMin_API.Infrastructure.Entity;
using MediatR;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace BankMin_API.Features.AccountUseCase.CreateAccount
{
    public record class CreateAccountCommand : IRequest<IResult>
    {
        public decimal Balance { get; init; }
        public Currency Currency { get; init; }
        public Guid UserId { get; init; }

    }
}
