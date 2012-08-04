namespace Data.Configuration
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;
    using Entities;

    internal class ActivationConfiguration : EntityTypeConfiguration<Activation>
    {
        public ActivationConfiguration()
        {
            HasKey(e => e.Id).Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.ConfirmationToken).HasMaxLength(EntitySettings.ConfirmationTokenLength);
            Property(e => e.ActivatedDate).HasColumnType("datetime2");
            HasRequired(e => e.User).WithMany(e => e.Activations).HasForeignKey(e => e.UserId).WillCascadeOnDelete(true);
        }
    }
}
