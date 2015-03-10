using School.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DataLayer
{
    public class ExaminationConfiguration : EntityTypeConfiguration<Examination>
    {
        public ExaminationConfiguration()
        {
            Property(ex => ex.StudentId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_ExaminationItem", 1) { IsUnique = true }));
            Property(ex => ex.Subject).IsRequired().HasMaxLength(50)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_ExaminationItem", 2) { IsUnique = true }));
            Property(ex => ex.DateTaken).IsRequired();
            Property(ex => ex.Guid).HasMaxLength(32).IsRequired().HasColumnName("Key");
            Property(ex => ex.MaximumMarks).IsRequired();
            Property(ex => ex.MarksObtained).IsRequired();
            Property(ex => ex.DateCreated).IsRequired();
            Property(ex => ex.DateModified).IsOptional();
            Ignore(s => s.ObjectState);
            Property(s => s.RowVersion).IsRowVersion();
        }
    }
}
