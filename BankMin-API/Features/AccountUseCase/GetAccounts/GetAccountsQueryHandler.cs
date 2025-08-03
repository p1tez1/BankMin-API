using BankMin_API.Services.IServices;
using MediatR;

namespace BankMin_API.Features.AccountUseCase.GetAccounts
{
    public class GetAccountsQueryHandler : IRequestHandler<GettAccountsQuery, IResult>
    {
        private readonly IAccountRepo _accountRepo;
        public GetAccountsQueryHandler(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }
        public async Task<IResult> Handle(GettAccountsQuery request, CancellationToken cancellationToken)
        {
            var accoutns = await _accountRepo.GetAccountsByUserIdAsync(request.userId);

            return Results.Ok(accoutns);
        }
    }
}
