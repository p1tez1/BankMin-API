using BankMin_API.Features.AccountUseCase.CreateAccount;
using BankMin_API.Features.AccountUseCase.GetAccounts;
using BankMin_API.Features.AccountUseCase.MoneyTransfer;
using MediatR;
using System.Security.Claims;

namespace BankMin_API.Features.AccountUseCase
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/account").WithTags("Account");

            group.MapPost("/create", async (CreateAccountModel model, HttpContext context, ISender sender) =>
            {
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? throw new UnauthorizedAccessException();

                var command = new CreateAccountCommand
                {
                    Balance = model.Balance,
                    Currency = model.Currency,
                    UserId = Guid.Parse(userId)
                };

                return await sender.Send(command);
            });

            group.MapGet("/get-my", async (GettAccountsQuery que, ISender sender) =>
            await sender.Send(que));

            group.MapPost("/send-funds", async (MoneyTransferModel model, HttpContext context, ISender sender) =>
            {
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? throw new UnauthorizedAccessException();

                var command = new MoneyTransferCommand
                {
                    UserId = Guid.Parse(userId),
                    FromId = model.FromId,
                    ToId = model.ToId,
                    Amount = model.Amount,
                };
                return await sender.Send(command);
            });
        }
    }
}
