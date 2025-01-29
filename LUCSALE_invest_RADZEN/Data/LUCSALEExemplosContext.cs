using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos;

namespace LUCSALEInvestRADZEN.Data
{
    public partial class LUCSALE_ExemplosContext : DbContext
    {
        public LUCSALE_ExemplosContext()
        {
        }

        public LUCSALE_ExemplosContext(DbContextOptions<LUCSALE_ExemplosContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos.Prod> Prods { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}