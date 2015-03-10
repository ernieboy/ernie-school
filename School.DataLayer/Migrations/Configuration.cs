using System;
using System.Data.Entity.Migrations;
using School.Models;

namespace School.DataLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

            //It's ok to destroy data if necessary during the migrations process. Note, don't do this in production! - migrations must be turned off in PROD.
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SchoolContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Students.AddOrUpdate(
                s => s.DateOfBirth,
                new Student
                {
                    DateOfBirth = new DateTime(1977, 6, 2),
                    FirstName = "Ernest",
                    LastName = "Fakudze",
                    Gender = GenderEnum.Male,
                    Guid = Guid.NewGuid().ToString("N"),
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    ExamsTaken = { 
                        new Examination
                    {
                        Subject = "Mathematics",
                        DateTaken = new DateTime(1995,07,23),
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Guid = Guid.NewGuid().ToString("N"),
                        MaximumMarks = 125,
                        MarksObtained = 80      

                    }
                    }
                }
                );
        }
    }
}