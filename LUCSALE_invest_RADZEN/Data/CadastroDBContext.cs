using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LUCSALEInvestRADZEN.Models.CadastroDB;

namespace LUCSALEInvestRADZEN.Data
{
    public partial class CadastroDBContext : DbContext
    {
        public CadastroDBContext()
        {
        }

        public CadastroDBContext(DbContextOptions<CadastroDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<LUCSALEInvestRADZEN.Models.CadastroDB.Aluno> Alunos { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}