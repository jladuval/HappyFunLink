namespace WebCore.Security.Interfaces
{
    using Entities;

    public interface IAccountService
    {
        void RegisterSiteUser(string firstName, string lastName, string email, string password, bool receiveEmails);

        bool ValidateSiteUser(string email, string password);

        void LoginUser(string email, bool remember);

        void LogoutUser();

        void GeneratePasswordResetToken(string email);

        bool IsPasswordResetTokenValid(string token);

        void ResetPasswordWithToken(string token, string newPassword);

        bool IsActivationTokenValid(string token);

        void ActivateUserEmail(string token);

        User GetUserByEmail(string email);

        void UpdateUser(User user);

        string GenerateToken();

        void ChangeEmail(string currentEmail, string newEmail);

        bool ChangePassword(string email, string oldPassword, string newPassword);
    }
}
