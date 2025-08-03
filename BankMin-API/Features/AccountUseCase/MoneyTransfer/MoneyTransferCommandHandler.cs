using BankMin_API.Features.AccountUseCase;
using BankMin_API.Features.AccountUseCase.MoneyTransfer;
using MediatR;
using System.ComponentModel.DataAnnotations;

public class MoneyTransferCommandHandler : IRequestHandler<MoneyTransferCommand, IResult>
{
    private readonly IMoneyTransferService _moneyTransferService;

    public MoneyTransferCommandHandler(IMoneyTransferService moneyTransferService)
    {
        _moneyTransferService = moneyTransferService;
    }

    public async Task<IResult> Handle(MoneyTransferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _moneyTransferService.SendMoney(request.UserId, request.FromId, request.ToId, request.Amount);
            return Results.Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.StatusCode(500);
        }
    }
}
