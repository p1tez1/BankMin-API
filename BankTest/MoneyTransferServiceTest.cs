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
    private readonly Mock<ICurrencyRateService> _currencyRateServiceMock;
    private readonly BankDbContext _dbContext;
    private readonly MoneyTransferService _service;

    public MoneyTransferServiceTests()
    {
        _accountRepoMock = new Mock<IAccountRepo>();
        _loggerMock = new Mock<ILogger<Account>>();
        _currencyRateServiceMock = new Mock<ICurrencyRateService>();

        var options = new DbContextOptionsBuilder<BankDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new BankDbContext(options);

        _service = new MoneyTransferService(
            _accountRepoMock.Object,
            _loggerMock.Object,
            _currencyRateServiceMock.Object,
            _dbContext);
    }

    [Fact]
    public async Task SendMoney_SuccessfulTransfer_ShouldUpdateBalances()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        decimal amount = 100;
        decimal exchangeRate = 36;

        var fromAccount = new Account
        {
            Id = fromId,
            UserId = userId,
            Balance = 200,
            Currency = Currency.UAH
        };

        var toAccount = new Account
        {
            Id = toId,
            UserId = Guid.NewGuid(),
            Balance = 50,
            Currency = Currency.UAH
        };

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);
        _accountRepoMock.Setup(r => r.GetAccountByidAsync(toId)).ReturnsAsync(toAccount);
        _currencyRateServiceMock.Setup(s => s.GetUsdToUahRateAsync()).ReturnsAsync(exchangeRate);

        // Act
        await _service.SendMoney(userId, fromId, toId, amount);

        // Assert
        Assert.Equal(100, fromAccount.Balance);
        Assert.Equal(150, toAccount.Balance);
    }

    [Fact]
    public async Task SendMoney_UnauthorizedUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        decimal amount = 50;

        var fromAccount = new Account
        {
            Id = fromId,
            UserId = Guid.NewGuid(), // чужий акаунт
            Balance = 100,
            Currency = Currency.UAH
        };

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);
        _currencyRateServiceMock.Setup(s => s.GetUsdToUahRateAsync()).ReturnsAsync(36); // необхідно

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.SendMoney(userId, fromId, toId, amount));
    }

    [Fact]
    public async Task SendMoney_InsufficientFunds_ThrowsValidationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        decimal amount = 150;

        var fromAccount = new Account
        {
            Id = fromId,
            UserId = userId,
            Balance = 100,
            Currency = Currency.UAH
        };

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);
        _currencyRateServiceMock.Setup(s => s.GetUsdToUahRateAsync()).ReturnsAsync(36); // обов'язково

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _service.SendMoney(userId, fromId, toId, amount));
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

        var fromAccount = new Account
        {
            Id = fromId,
            UserId = userId,
            Balance = 100,
            Currency = Currency.UAH
        };

        _accountRepoMock.Setup(r => r.GetAccountByidAsync(fromId)).ReturnsAsync(fromAccount);
        _accountRepoMock.Setup(r => r.GetAccountByidAsync(toId)).ReturnsAsync((Account)null);
        _currencyRateServiceMock.Setup(s => s.GetUsdToUahRateAsync()).ReturnsAsync(36);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.SendMoney(userId, fromId, toId, amount));
        Assert.Equal("Recipient account not found", ex.Message);
    }
}
