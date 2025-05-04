using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using projectStudent.Models;

namespace projectStudent.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base("name=OracleDbContext")
        {
        }

        public DbSet<Students> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Se o nome do schema for diferente do padrão do usuário, você pode configurar aqui.
            modelBuilder.HasDefaultSchema("SYS"); // Usando o schema SYS
            modelBuilder.Entity<Students>().ToTable("TB_STUDENTS");

            // Configurações adicionais do modelo, se necessário.
            base.OnModelCreating(modelBuilder);

        }

    }
}