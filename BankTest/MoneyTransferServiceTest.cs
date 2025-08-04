using BankMin_API.Infrastructure;
using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services;
using BankMin_API.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;

public class MoneyTransferServiceTests
{
    private readonly Mock<IAccountRepo> _accountRepoMock;
    private readonly Mock<ILogger<Account>> _loggerMock;
    private readonly BankDbContext _dbContext;
    private readonly MoneyTransferService _service;

    public MoneyTransferServiceTests()
    {
        _accountRepoMock = new Mock<IAccountRepo>();
        _loggerMock = new Mock<ILogger<Account>>();

        var options = new DbContextOptionsBuilder<BankDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new BankDbContext(options);

        _service = new MoneyTransferService(_accountRepoMock.Object, _loggerMock.Object, _dbContext);
    }

    [Fact]
    public async Task SendMoney_SuccessfulTransfer_ShouldUpdateBalances()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        decimal amount = 100;

        var fromAccount = new Account { Id = fromId, UserId = userId, Balance = 200 };
        var toAccount = new Account { Id = toId, UserId = Guid.NewGuid(), Balance = 50 };

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);
        _accountRepoMock.Setup(r => r.GetAccountByidAsync(toId)).ReturnsAsync(toAccount);

        // Act
        await _service.SendMoney(userId, fromId, toId, amount);

        // Assert
        Assert.Equal(100, fromAccount.Balance);
        Assert.Equal(150, toAccount.Balance);

        _accountRepoMock.Verify(r => r.GetAccountByidAsync(fromId), Times.Once);
        _accountRepoMock.Verify(r => r.GetAccountByidAsync(toId), Times.Once);
    }

    [Fact]
    public async Task SendMoney_UnauthorizedUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        decimal amount = 50;

        var fromAccount = new Account { Id = fromId, UserId = Guid.NewGuid(), Balance = 100 }; // UserId не співпадає з userId

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.SendMoney(userId, fromId, toId, amount));
    }

    [Fact]
    public async Task SendMoney_InsufficientFunds_ThrowsValidationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        decimal amount = 150;

        var fromAccount = new Account { Id = fromId, UserId = userId, Balance = 100 };

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(() => _service.SendMoney(userId, fromId, toId, amount));
        Assert.Equal("Insufficient funds", ex.Message);
    }

    [Fact]
    public async Task SendMoney_RecipientAccountNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        decimal amount = 50;

        var fromAccount = new Account { Id = fromId, UserId = userId, Balance = 100 };

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);
        _accountRepoMock.Setup(r => r.GetAccountByidAsync(toId)).ReturnsAsync((Account)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.SendMoney(userId, fromId, toId, amount));
        Assert.Equal("Recipient account not found", ex.Message);
    }
}
