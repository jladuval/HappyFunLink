namespace WebCore.Security
{
    using System;
    using System.Web.Security;
    using Domain.Mailing.Interfaces;
    using Entities;
    using Interfaces;

    public class AccountMembershipService : IAccountService
    {
        private const string UserRole = "Users";

        private readonly MembershipProviderBase _membership;

        private readonly RoleProviderBase _roles;

        private readonly IEmailManager _emailManager;

        public AccountMembershipService(
            MembershipProviderBase membership,
            RoleProviderBase roles,
            IEmailManager emailManager)
        {
            _membership = membership;
            _roles = roles;
            _emailManager = emailManager;
        }

        public void RegisterSiteUser(string firstName, string lastName, string email, string password, bool receiveEmails)
        {
            var activationToken = _membership.CreateUser(firstName, lastName, email, password, receiveEmails);
            _roles.AddUserToRole(email, UserRole);

            SendActivationEmail(email, activationToken);
        }
       
        public bool ValidateSiteUser(string email, string password)
        {
            return _membership.ValidateUser(email, password);
        }

        public void LoginUser(string email, bool remember)
        {
            var user = _membership.GetUser(email);
            if (user == null) throw new ArgumentException("email");
            _membership.LoginUser(email);
        }

        public void LogoutUser()
        {
            FormsAuthentication.SignOut();
        }

        public void GeneratePasswordResetToken(string email)
        {
            var token = _membership.GeneratePasswordResetToken(email);
            var user = _membership.GetUser(email);
            //_emailManager.SendResetPassword(user, token);
        }

        public bool IsPasswordResetTokenValid(string token)
        {
            return _membership.IsPasswordResetTokenValid(token);
        }

        public void ResetPasswordWithToken(string token, string newPassword)
        {
            _membership.ResetPasswordWithToken(token, newPassword);
        }

        public bool IsActivationTokenValid(string token)
        {
            return _membership.IsActivationTokenValid(token);
        }

        public void ActivateUserEmail(string token)
        {
            _membership.ActivateUserEmail(token);
            var email = _membership.GetUsernameFromActivationToken(token);
            SendWelcomeEmail(email);
            LoginUser(email,
                false);
        }

        public User GetUserByEmail(string email)
        {
            return _membership.GetUser(email);
        }

        public void UpdateUser(User user)
        {
            _membership.UpdateUser(user);
        }

        public string GenerateToken()
        {
            return _membership.GenerateToken();
        }

        public void ChangeEmail(string currentEmail, string newEmail)
        {
            string activationToken = _membership.ChangeEmail(currentEmail, newEmail);
            SendActivationEmail(newEmail, activationToken);
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            return _membership.ChangePassword(email, oldPassword, newPassword);
        }

        private void SendWelcomeEmail(string email)
        {
            var user = _membership.GetUser(email);
            _emailManager.SendWelcomeEmail(user);
        }

        private void SendActivationEmail(string email, string activationToken)
        {
            var user = _membership.GetUser(email);
            _emailManager.SendActivationEmail(user, activationToken);
        }
    }
}
