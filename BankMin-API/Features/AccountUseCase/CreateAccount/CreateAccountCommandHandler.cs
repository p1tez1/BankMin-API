using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;
using MediatR;

namespace BankMin_API.Features.AccountUseCase.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, IResult>
    {
        private readonly IAccountRepo _accountRepo;
        private readonly ILogger<Account> _logger;
        public CreateAccountCommandHandler(IAccountRepo accountRepo, ILogger<Account> logger)
        {
            _accountRepo = accountRepo;
            _logger = logger;
        }
        public async Task<IResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Balance = request.Balance,
                Currency = request.Currency,
                UserId = request.UserId,
            };
            await _accountRepo.CreateAccountAsync(account);

            _logger.LogInformation($"User {account.UserId} successfully create Account");

            return Results.Created();

        }
    }
}
