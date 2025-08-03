namespace BankMin_API.Features.AccountUseCase.MoneyTransfer
{
    public class MoneyTransferModel
    {
        public Guid FromId { get; init; }
        public Guid ToId { get; init; }
        public decimal Amount { get; init; }
    }
}
