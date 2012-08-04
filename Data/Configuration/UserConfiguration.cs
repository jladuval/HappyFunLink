namespace Data.Configuration
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;
    using Entities;

    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(e => e.Id).Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.FirstName).HasMaxLength(EntitySettings.FirstNameLength);
            Property(e => e.LastName).HasMaxLength(EntitySettings.LastNameLength);
            Property(e => e.Email).IsRequired().HasMaxLength(EntitySettings.EmailLength);
            Property(e => e.CreatedDate).HasColumnType("datetime2");
            HasMany(e => e.Roles).WithMany(e => e.Users).Map(
                c =>
                {
                    c.MapLeftKey("UserId");
                    c.MapRightKey("RoleId");
                    c.ToTable("UsersInRoles");
                });
        }
    }
}
