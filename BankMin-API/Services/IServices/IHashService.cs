namespace BankMin_API.Services.IServices
{
    public interface IHashService
    {
        public string Hash(string input);
        public bool Verify(string input, string hashed);
    }
}
