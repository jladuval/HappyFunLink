namespace WebCore.Security.Interfaces
{
    public interface IMembershipCrypto
    {
        bool VerifyHashedPassword(string hashedPassword, string password);

        string HashPassword(string password);

        string GenerateToken();
    }
}
