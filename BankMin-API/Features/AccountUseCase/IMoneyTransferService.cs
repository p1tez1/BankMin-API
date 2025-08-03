namespace BankMin_API.Features.AccountUseCase
{
    public interface IMoneyTransferService
    {
        Task SendMoney(Guid userId, Guid fromId, Guid toId, decimal amount);
    }
}
