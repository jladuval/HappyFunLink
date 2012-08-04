using System.Data.Entity;
using Data.Configuration;

namespace Data.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new SiteRegistrationConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ActivationConfiguration());

        }
    }
}
