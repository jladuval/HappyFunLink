namespace Entities
{
    using System;
    public class SiteRegistration
    {
        public int Id { get; set; }

        public virtual User User { get; set; }

        public string Password { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        // Invalid sign in attempts
        public bool IsLockedOut { get; set; }

        public DateTime? LastLockoutDate { get; set; }

        public int FailedPasswordAttemptCount { get; set; }

        public DateTime? LastFailedPasswordAttemptDate { get; set; }

        public DateTime? FailedPasswordAttemptWindowStart { get; set; }

        // Password reset
        public string PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiredDate { get; set; }
    }
}
