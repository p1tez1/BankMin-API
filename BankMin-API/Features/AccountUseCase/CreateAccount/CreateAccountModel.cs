using BankMin_API.Infrastructure.Entity;

namespace BankMin_API.Features.AccountUseCase.CreateAccount
{
    public class CreateAccountModel
    {
        public decimal Balance { get; init; }
        public Currency Currency { get; init; }
    }
}
