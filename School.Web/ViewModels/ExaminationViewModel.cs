using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Web.ViewModels
{
    public class ExaminationViewModel : BaseViewModel, IObjectWithState
    {
        public int ExaminationId { get; set; }
        public string Subject { get; set; }
        public int MaximumMarks { get; set; }
        public double MarksObtained { get; set; }

        public string DateTaken { get; set; }

        public int StudentId { get; set; }

    }
}