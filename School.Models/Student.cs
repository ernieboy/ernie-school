using System;
using System.Collections.Generic;


namespace School.Models
{
    public class Student : BaseObjectWithState, IObjectWithState
    {
        public Student()
        {
            ExamsTaken = new List<Examination>();
        }
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public GenderEnum Gender { get; set; }

        public DateTime  DateOfBirth { get; set; }

        public virtual ICollection<Examination> ExamsTaken { get; set; }        

    }
}