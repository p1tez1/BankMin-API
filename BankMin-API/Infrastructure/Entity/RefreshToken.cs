namespace BankMin_API.Infrastructure.Entity
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = new Guid();
        public string HeshRefreshTocken { get; set; }
        public bool Valid { get; set; } = true;
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
