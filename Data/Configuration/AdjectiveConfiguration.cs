using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Entities;

namespace Data.Configuration
{
    internal class AdjectiveConfiguration : EntityTypeConfiguration<Adjective>
    {
        public AdjectiveConfiguration() {
            HasKey(e => e.Id).Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    
    }
}
