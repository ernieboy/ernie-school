using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Models
{
    public class Examination :   BaseObjectWithState, IObjectWithState
    {
        public int ExaminationId { get; set; }
        public string Subject { get; set; }
        public int MaximumMarks  { get; set; }
        public double MarksObtained { get; set; }

        public DateTime DateTaken { get; set; }

        public int StudentId { get; set; }
        public Student  Student { get; set; }

    }
}
