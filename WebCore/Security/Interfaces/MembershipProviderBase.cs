namespace WebCore.Security.Interfaces
{
    using System.Web.Security;
    using Entities;

    public abstract class MembershipProviderBase : MembershipProvider
    {
        #region Public Properties

        /// <summary>
        /// Gets the days amount while reset password token is valid.
        /// </summary>
        public abstract int ResetPasswordTokenPeriod { get; }

        public abstract int Timeout { get; } /*In minutes*/

        #endregion

        #region Public Methods and Operators

        public abstract bool IsUserActivated(string email);

        // Site registration
        public abstract string CreateUser(string firstName, string lastName, string email, string password, bool receiveEmails);

        public abstract bool DeleteUser(string email);

        public abstract string GeneratePasswordResetToken(string email);

        public abstract void LoginUser(string email);

        public abstract bool IsPasswordResetTokenValid(string token);

        public abstract bool IsActivationTokenValid(string token);

        public abstract void ActivateUserEmail(string token);

        public abstract bool LockUser(string email);

        public abstract void ResetPassword(User user, string newPassword);

        public abstract void ResetPasswordWithToken(string token, string newPassword);

        public abstract string GetUsernameFromActivationToken(string token);

        public abstract User GetUser(string email);

        public abstract void UpdateUser(User user);

        public abstract string GenerateToken();

        public abstract string ChangeEmail(string currentEmail, string newEmail);

        #endregion
    }
}
