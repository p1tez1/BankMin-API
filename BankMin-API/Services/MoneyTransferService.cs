using BankMin_API.Features.AccountUseCase;
using BankMin_API.Infrastructure;
using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;
using System.ComponentModel.DataAnnotations;

namespace BankMin_API.Services
{
    public class MoneyTransferService : IMoneyTransferService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly ILogger<Account> _logger;
        private readonly BankDbContext _dBcontext;
        public MoneyTransferService(IAccountRepo accountRepo, ILogger<Account> logger, BankDbContext dbContext)
        {
            _accountRepo = accountRepo;
            _logger = logger;
            _dBcontext = dbContext;
        }
        public async Task SendMoney(Guid userId, Guid fromId, Guid toId, decimal amount)
        {
            using var transaction = await _dBcontext.Database.BeginTransactionAsync();

            try
            {
                var fromAccount = await _accountRepo.GetAccountByidAsync(fromId);

                if (fromAccount == null || fromAccount.UserId != userId)
                {
                    _logger.LogWarning("Unauthorized money transfer attempt by user {UserId}", userId);
                    throw new UnauthorizedAccessException();
                }

                if (fromAccount.Balance < amount)
                {
                    _logger.LogInformation("Not enough funds in account {FromId} for user {UserId}", fromId, userId);
                    throw new ValidationException("Insufficient funds");
                }

                var toAccount = await _accountRepo.GetAccountByidAsync(toId);

                if (toAccount == null)
                {
                    _logger.LogWarning("Invalid recipient account {ToId}", toId);
                    throw new KeyNotFoundException("Recipient account not found");
                }

                fromAccount.Balance -= amount;
                toAccount.Balance += amount;

                await _dBcontext.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("User {UserId} sent {Amount} from {FromId} to {ToId}", userId, amount, fromId, toId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Money transfer failed");
                throw;
            }
        }

    }
}

