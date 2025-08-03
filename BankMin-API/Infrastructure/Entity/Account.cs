namespace BankMin_API.Infrastructure.Entity
{
    public class Account
    {
        public Guid Id { get; set; } = new Guid();
        public decimal Balance { get; set; } = 0;
        public Currency Currency { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
