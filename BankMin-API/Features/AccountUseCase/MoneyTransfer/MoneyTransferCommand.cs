using MediatR;

namespace BankMin_API.Features.AccountUseCase.MoneyTransfer
{
    public record class MoneyTransferCommand() : IRequest<IResult>
    {
        public Guid UserId { get; init; }
        public Guid FromId { get; init; }
        public Guid ToId { get; init; }
        public decimal Amount { get; init; }
    }
}
