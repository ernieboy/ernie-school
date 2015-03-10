using System.Data.Entity;
using School.Models;
using School.DataLayer.Migrations;

namespace School.DataLayer
{
    public class SchoolContext : DbContext
    {

        public SchoolContext() : base("DefaultConnection")
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Examination> Exams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SchoolContext, Configuration>());   
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new ExaminationConfiguration());

            //Do the stuff for ASP.Net identity
            base.OnModelCreating(modelBuilder);    
        }
    }
}