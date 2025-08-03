using MediatR;
using System.Reflection;
using System.Security.Claims;

namespace BankMin_API.Features.AccountUseCase.GetAccounts
{
    public record class GettAccountsQuery : IRequest<IResult>
    {
        public Guid userId { get; init; }

        public static ValueTask<GettAccountsQuery> BindAsync(HttpContext context, ParameterInfo parameter)
        {
            string id = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException();

            var query = new GettAccountsQuery
            {
                userId = Guid.Parse(id)
            };

            return ValueTask.FromResult(query);
        }
    }
}
