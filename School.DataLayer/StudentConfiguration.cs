using System.Data.Entity.ModelConfiguration;
using School.Models;

namespace School.DataLayer
{
    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            Property(s => s.FirstName).HasMaxLength(30).IsRequired();
            Property(s => s.LastName).HasMaxLength(30).IsRequired();
            Property(s => s.Guid).HasMaxLength(32).IsRequired().HasColumnName("Key");
            Property(s => s.DateOfBirth).IsRequired();
            Property(s => s.DateCreated).IsRequired();
            Property(s => s.DateModified).IsOptional();
            Ignore(s => s.ObjectState);
            Property(s => s.RowVersion).IsRowVersion();
        }
    }
}