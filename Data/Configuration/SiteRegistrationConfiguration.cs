namespace Data.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using Entities;

    internal class SiteRegistrationConfiguration : EntityTypeConfiguration<SiteRegistration>
    {
        public SiteRegistrationConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Password).HasMaxLength(EntitySettings.PasswordLength);
            Property(e => e.LastPasswordChangedDate).HasColumnType("datetime2");
            Property(e => e.LastLockoutDate).HasColumnType("datetime2");
            Property(e => e.LastFailedPasswordAttemptDate).HasColumnType("datetime2");
            Property(e => e.FailedPasswordAttemptWindowStart).HasColumnType("datetime2");
            Property(e => e.PasswordResetToken).HasMaxLength(EntitySettings.PasswordResetTokenLength);
            Property(e => e.PasswordResetTokenExpiredDate).HasColumnType("datetime2");
            HasRequired(e => e.User).WithOptional(e => e.SiteRegistration).WillCascadeOnDelete();
        }
    }
}
