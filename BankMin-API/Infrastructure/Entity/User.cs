namespace BankMin_API.Infrastructure.Entity
{
    public class User
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
