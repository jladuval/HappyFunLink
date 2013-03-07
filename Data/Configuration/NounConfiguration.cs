using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Entities;

namespace Data.Configuration
{
    internal class NounConfiguration : EntityTypeConfiguration<Noun>
    {
        public NounConfiguration() {
            HasKey(e => e.Id).Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    
    }
}
