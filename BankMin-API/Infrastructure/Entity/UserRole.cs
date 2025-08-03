namespace BankMin_API.Infrastructure.Entity
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }

        public DateTime CreatedAt = DateTime.Now;
    
    }
}
