namespace BankMin_API.Infrastructure.Entity
{
    public class Role
    {
        public Guid Id { get; set; } = new Guid();
        public string RoleName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
