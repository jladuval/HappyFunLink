namespace WebCore.Security
{
    using System;
    using System.Collections.Specialized;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using Data.Interfaces;
    using Entities;
    using Exceptions;
    using Interfaces;

    public class ExtendedMembershipProvider : MembershipProviderBase
    {
        #region Constants and Fields

        private const string ApplicationNameProperty = "applicationName";

        private const string MaxInvalidPasswordAttemptsProperty = "maxInvalidPasswordAttempts";

        private const string MinRequiredNonalphanumericCharactersProperty = "minRequiredNonalphanumericCharacters";

        private const string MinRequiredPasswordLengthProperty = "minRequiredPasswordLength";

        private const string PasswordAttemptWindowProperty = "passwordAttemptWindow";

        private const string ResetPasswordTokenPeriodProperty = "resetPasswordTokenPeriod";

        private const string TimeoutProperty = "timeout";

        private const int DefaultMaxInvalidPasswordAttempts = 0x100;

        private const int MaxPasswordLength = 0x80;

        private const int DefaultminRequiredPasswordLength = 8;

        private readonly IRepository<Activation> _activations;

        private readonly IDateTimeProvider _dateTime;

        private readonly IRepository<SiteRegistration> _siteRegistrations;

        private readonly IMembershipCrypto _crypto;

        private readonly IUnitOfWork _uow;

        private readonly IRepository<User> _users;

        private int _maxInvalidPasswordAttempts;

        private int _minRequiredNonalphanumericCharacters;

        private int _minRequiredPasswordLength;

        private int _passwordAttemptWindow;

        private int _resetPasswordTokenPeriod;

        private int _timeout;

        #endregion

        #region Constructors and Destructors

        public ExtendedMembershipProvider()
            : this(
                DependencyResolver.Current.GetService<IUnitOfWork>(),
                DependencyResolver.Current.GetService<IRepository<User>>(),
                DependencyResolver.Current.GetService<IRepository<Activation>>(),
                DependencyResolver.Current.GetService<IRepository<SiteRegistration>>(),
                DependencyResolver.Current.GetService<IMembershipCrypto>(),
                DependencyResolver.Current.GetService<IDateTimeProvider>())
        {
        }

        public ExtendedMembershipProvider(
            IUnitOfWork uow,
            IRepository<User> users,
            IRepository<Activation> activations,
            IRepository<SiteRegistration> siteRegistrations,
            IMembershipCrypto crypto,
            IDateTimeProvider dateTime)
        {
            _uow = uow;
            _users = users;
            _activations = activations;
            _siteRegistrations = siteRegistrations;
            _crypto = crypto;
            _dateTime = dateTime;
        }

        #endregion

        #region Public Properties

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get
            {
                return true;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return false;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return _maxInvalidPasswordAttempts == 0
                           ? DefaultMaxInvalidPasswordAttempts
                           : _maxInvalidPasswordAttempts;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return _minRequiredNonalphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return _minRequiredPasswordLength == 0
                        ? DefaultminRequiredPasswordLength
                        : _minRequiredPasswordLength;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return _passwordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return MembershipPasswordFormat.Hashed;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return true;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int ResetPasswordTokenPeriod
        {
            get
            {
                return _resetPasswordTokenPeriod;
            }
        }

        public override int Timeout
        {
            get { return _timeout; }
        }

        #endregion

        #region Public Methods and Operators

        public override void Initialize(string name, NameValueCollection config)
        {
            ErrorUtility.CheckIfNotNull(config, "config");

            if (string.IsNullOrEmpty(name))
                name = GetType().Name;

            ApplicationName = config[ApplicationNameProperty];
            int.TryParse(config[MaxInvalidPasswordAttemptsProperty], out _maxInvalidPasswordAttempts);
            int.TryParse(config[PasswordAttemptWindowProperty], out _passwordAttemptWindow);
            int.TryParse(config[MinRequiredPasswordLengthProperty], out _minRequiredPasswordLength);
            int.TryParse(
                config[MinRequiredNonalphanumericCharactersProperty], out _minRequiredNonalphanumericCharacters);
            int.TryParse(config[ResetPasswordTokenPeriodProperty], out _resetPasswordTokenPeriod);
            int.TryParse(config[TimeoutProperty], out _timeout);

            base.Initialize(name, config);
        }

        public override bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            ErrorUtility.CheckArgument(email, "email");
            ErrorUtility.CheckArgument(oldPassword, "oldPassword");
            ErrorUtility.CheckArgument(newPassword, "newPassword");

            var user = ExistingUser(email);

            var siteRegistration = user.SiteRegistration;
            ErrorUtility.CheckIfNotNull(siteRegistration, email);

            // TODO: Early abort if user is locked out. 

            var hashedPassword = siteRegistration.Password;
            var valid = _crypto.VerifyHashedPassword(hashedPassword, oldPassword);

            var currentTime = _dateTime.Now;

            if (!valid)
            {
                LoginFailed(siteRegistration, currentTime);
                _uow.Commit();
                return false;
            }

            ResetLockout(siteRegistration);
            var newPasswordHash = _crypto.HashPassword(newPassword);
            siteRegistration.Password = newPasswordHash;
            siteRegistration.LastPasswordChangedDate = currentTime;
            _uow.Commit();
            return true;
        }

        public override string ChangeEmail(string currentEmail, string newEmail)
        {
            ErrorUtility.CheckArgument(currentEmail, "currentEmail");
            ErrorUtility.CheckArgument(newEmail, "newEmail");
            AssertUserDoesNotExist(newEmail);

            var user = ExistingUser(currentEmail);
            user.Email = newEmail;
            var activation = CreateActivation(user);
            _uow.Commit();

            return activation.ConfirmationToken;
        }

        public override void LoginUser(string email)
        {
            var user = GetUser(email);
            ErrorUtility.CheckIfNotNull(user, email);
            _uow.Commit();
        }

        public override bool ChangePasswordQuestionAndAnswer(
            string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override string CreateUser(string firstName, string lastName, string email, string password, bool receiveEmails)
        {
            ErrorUtility.CheckArgument(firstName, "firstName");
            ErrorUtility.CheckArgument(lastName, "lastName");
            ErrorUtility.CheckArgument(email, "email");
            ErrorUtility.CheckArgument(password, "password");
            AssertUserDoesNotExist(email);

            var passwordHash = _crypto.HashPassword(password);

            var currentTime = _dateTime.Now;

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                CreatedDate = currentTime,
            };

            _users.Create(user);

            var registration = new SiteRegistration
            {
                Password = passwordHash,
                LastPasswordChangedDate = currentTime,
                User = user
            };
            _siteRegistrations.Create(registration);

            var activation = CreateActivation(user);

            _uow.Commit();

            return activation.ConfirmationToken;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string email)
        {
            ErrorUtility.CheckArgument(email, "email");
            var user = GetUser(email);
            if (null == user)
                return false;

            var activation = _activations.FindById(user.Id);
            if (null != activation)
                _activations.Delete(activation);

            var siteRegistration = _siteRegistrations.FindById(user.Id);
            if (null != siteRegistration)
                _siteRegistrations.Delete(siteRegistration);

            _uow.Commit();
            return true;
        }

        public override MembershipUserCollection FindUsersByEmail(
            string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(
            string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override string GeneratePasswordResetToken(string email)
        {
            ErrorUtility.CheckArgument(email, "email");

            var user = GetUser(email);
            if (null == user)
                throw new PasswordResetException(PasswordResetStatus.UserNotFound);

            var siteRegistration = user.SiteRegistration;
            var token = _crypto.GenerateToken();
            siteRegistration.PasswordResetToken = token;
            siteRegistration.PasswordResetTokenExpiredDate = _dateTime.Now.AddDays(_resetPasswordTokenPeriod);
            _uow.Commit();

            return token;
        }

        public override bool IsPasswordResetTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            var registration =
                _siteRegistrations.Find(e => e.PasswordResetToken == token).SingleOrDefault();
            var valid = null != registration && registration.PasswordResetToken.Equals(token, StringComparison.Ordinal)
                        && registration.PasswordResetTokenExpiredDate > _dateTime.Now;
            return valid;
        }

        public override bool IsActivationTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            var activation = _activations.Find(e => e.ConfirmationToken == token).Include(e => e.User).SingleOrDefault();
            var valid = null != activation
                        && activation.ConfirmationToken.Equals(token, StringComparison.Ordinal);
            return valid;
        }

        public override void ActivateUserEmail(string token)
        {
            ErrorUtility.CheckArgument(token, "token");

            var activation = _activations.Find(e => e.ConfirmationToken == token).Include(e => e.User).SingleOrDefault();
            if (null == activation)
                throw new ActivationException(ActivationStatus.TokenNotFound);

            activation.ActivatedDate = _dateTime.Now;
            _uow.Commit();
        }

        public override void ResetPasswordWithToken(string token, string newPassword)
        {
            ErrorUtility.CheckArgument(token, "token");
            ErrorUtility.CheckArgument(newPassword, "newPassword");

            var newPasswordHash = _crypto.HashPassword(newPassword);

            var registration = _siteRegistrations.Find(e => e.PasswordResetToken == token).SingleOrDefault();
            if (null == registration)
                throw new PasswordResetException(PasswordResetStatus.TokenNotFound);
            if (registration.PasswordResetTokenExpiredDate < _dateTime.Now)
                throw new PasswordResetException(PasswordResetStatus.TokenExpired);

            registration.PasswordResetTokenExpiredDate = _dateTime.Now;
            registration.Password = newPasswordHash;
            registration.LastPasswordChangedDate = _dateTime.Now;
            registration.PasswordResetToken = null;
            registration.PasswordResetTokenExpiredDate = null;
            _uow.Commit();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool LockUser(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw ErrorUtility.CreateArgumentNullOrEmptyException("email");

            var user = _users.Find(e => e.Email == email).SingleOrDefault();
            if (null == user) return false;

            var registration = _siteRegistrations.FindById(user.Id);
            if (null == registration || registration.IsLockedOut) return false;

            registration.IsLockedOut = true;
            _uow.Commit();
            return true;
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override void ResetPassword(User user, string newPassword)
        {
            if (null == user)
                throw new ArgumentNullException("user");

            if (string.IsNullOrEmpty(newPassword))
                throw ErrorUtility.CreateArgumentNullOrEmptyException("newPassword");

            var newPasswordHash = _crypto.HashPassword(newPassword);

            var registration = _siteRegistrations.FindById(user.Id);
            ErrorUtility.CheckIfNotNull(registration, user.Email);
            registration.Password = newPasswordHash;
            registration.LastPasswordChangedDate = _dateTime.Now;
            registration.PasswordResetToken = null;
            registration.PasswordResetTokenExpiredDate = null;
            _uow.Commit();
        }

        public override bool UnlockUser(string email)
        {
            ErrorUtility.CheckArgument(email, "email");
            var user = GetUser(email);
            if (null == user) return false;

            var registration = _siteRegistrations.FindById(user.Id);
            if (!registration.IsLockedOut) return false;

            registration.IsLockedOut = false;
            _uow.Commit();
            return true;
        }

        public override void UpdateUser(User user)
        {
            _users.Update(user);
            _uow.Commit();
        }

        public override bool ValidateUser(string email, string password)
        {
            ErrorUtility.CheckArgument(email, "email");
            ErrorUtility.CheckArgument(password, "password");

            var user = GetUser(email);
            if (null == user) return false;

            var registration = user.SiteRegistration;
            if (null == registration || registration.IsLockedOut) return false;

            var passwordHash = registration.Password;
            var valid = passwordHash != null && _crypto.VerifyHashedPassword(passwordHash, password);
            if (valid)
            {
                if (!IsUserActivated(user)) throw new ActivationException(ActivationStatus.UserNotActivated);
                ResetLockout(registration);
                _uow.Commit();
                return true;
            }

            _uow.Commit();
            return false;
        }

        public override User GetUser(string email)
        {
            ErrorUtility.CheckArgument(email, "email");

            var user = _users.Find(e => e.Email == email).SingleOrDefault();

            return user;
        }

        public override string GenerateToken()
        {
            return _crypto.GenerateToken();
        }
        
        public override string GetUsernameFromActivationToken(string token)
        {
            var activation = _activations.Find(x => x.ConfirmationToken == token).Include(x => x.User).Single();
            if (activation.User == null)
                throw new ArgumentException("Nonexistent token: " + token);
            return activation.User.Email;
        }

        public override bool IsUserActivated(string email)
        {
            ErrorUtility.CheckArgument(email, "email");
            var user = ExistingUser(email);
            return IsUserActivated(user);
        }
        #endregion

        #region Helper Methods
        private bool UserExists(string email)
        {
            return _users.Find(x => x.Email == email).Any();
        }

        private void AssertUserDoesNotExist(string email)
        {
            if (UserExists(email))
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);
        }

        private bool IsUserActivated(User user)
        {
            return user.Activations.FirstOrDefault().ActivatedDate != null;
        }

        private Activation CreateActivation(User user)
        {
            var activation = new Activation { ConfirmationToken = _crypto.GenerateToken(), User = user };
            _activations.Create(activation);

            return activation;
        }

        private User ExistingUser(string email)
        {
            var user = GetUser(email);
            ErrorUtility.CheckIfNotNull(user, email);
            return user;
        }

        private void LoginFailed(SiteRegistration registration, DateTime? now = null)
        {
            DateTime currentTime = now ?? _dateTime.Now;
            registration.FailedPasswordAttemptWindowStart =
                registration.FailedPasswordAttemptWindowStart ?? currentTime;
            registration.FailedPasswordAttemptCount += 1;
            registration.LastFailedPasswordAttemptDate = currentTime;

            var limitReached = registration.FailedPasswordAttemptCount >= MaxInvalidPasswordAttempts;
            var inCurrentWindow = registration.FailedPasswordAttemptWindowStart.Value
                                  >= _dateTime.Now - TimeSpan.FromMinutes(PasswordAttemptWindow);
            if (limitReached && inCurrentWindow)
                registration.IsLockedOut = true;
        }

        private void ResetLockout(SiteRegistration siteRegistration)
        {
            siteRegistration.FailedPasswordAttemptCount = 0;
            siteRegistration.FailedPasswordAttemptWindowStart = null;
        }

        #endregion
    }
}
