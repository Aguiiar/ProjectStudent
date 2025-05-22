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

        public DbSet<Models.Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SYS"); 
            modelBuilder.Entity<Models.Student>().ToTable("TB_STUDENT");

            base.OnModelCreating(modelBuilder);

        }

    }
}