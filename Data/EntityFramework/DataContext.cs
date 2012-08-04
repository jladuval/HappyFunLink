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
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new SiteRegistrationConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new NounConfiguration());

            modelBuilder.Configurations.Add(new AdjectiveConfiguration());
            modelBuilder.Configurations.Add(new LinkConfiguration());
            modelBuilder.Configurations.Add(new HappyLinkConfiguration());
        }
    }
}
