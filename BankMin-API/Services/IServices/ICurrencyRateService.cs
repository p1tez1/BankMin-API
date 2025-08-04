namespace BankMin_API.Services.IServices
{
    public interface ICurrencyRateService
    {
        Task<decimal> GetUsdToUahRateAsync();
    }
}
