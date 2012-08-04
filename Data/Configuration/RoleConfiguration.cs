namespace Data.Configuration
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;
    using Entities;

    internal class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            HasKey(e => e.Id).Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.RoleName).HasMaxLength(EntitySettings.RoleNameLength).IsRequired();
            Property(e => e.Description).HasMaxLength(EntitySettings.RoleDescriptionLength);
        }
    }
}
