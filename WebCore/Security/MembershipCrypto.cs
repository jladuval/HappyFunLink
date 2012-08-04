namespace WebCore.Security
{
    using Interfaces;

    public class MembershipCrypto : IMembershipCrypto
    {
        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            return MembershipCryptoUtility.VerifyHashedPassword(hashedPassword, password);
        }

        public string HashPassword(string password)
        {
            return MembershipCryptoUtility.HashPassword(password);
        }

        public string GenerateToken()
        {
            return MembershipCryptoUtility.GenerateToken();
        }
    }
}
