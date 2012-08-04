﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
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