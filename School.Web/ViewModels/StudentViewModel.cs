using System;
using System.ComponentModel.DataAnnotations;
using School.Models;
using System.Collections.Generic;

namespace School.Web.ViewModels
{
    public class StudentViewModel : BaseViewModel, IObjectWithState
    {

        public StudentViewModel()
        {
            ExamsTaken = new List<ExaminationViewModel>();
            ExamItemsToDelete = new List<int>();
        }
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Server: Firstname is required")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Server: Last name is required")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Server: Gender is required")]
        [Range(1, 3), Display(Name = "Gender")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Server: Date of birth is required")]
        [Display(Name = "Date of birth")]
        public string DateOfBirth { get; set; }


        public List<ExaminationViewModel> ExamsTaken { get; set; }

        public List<int> ExamItemsToDelete { get; set; }

      
    }
}