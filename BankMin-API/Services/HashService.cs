using BankMin_API.Services.IServices;

namespace BankMin_API.Services
{
    public class HashService : IHashService
    {
        public string Hash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public bool Verify(string input, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashed);
        }

    }
}
