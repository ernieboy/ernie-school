using School.Models;
using System;
using System.Globalization;
namespace School.Web.ViewModels
{
    public static class Helpers
    {
        public static StudentViewModel CreateStudentViewModelFromStudent(Student student)
        {
            var model = new StudentViewModel
            {
                DateCreated = student.DateCreated.ToString(CultureInfo.CurrentCulture),
                DateModified = student.DateModified.ToString(CultureInfo.CurrentCulture),
                DateOfBirth = student.DateOfBirth.ToString(CultureInfo.CurrentCulture),
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = (int)student.Gender,
                Guid = student.Guid,
                StudentId = student.StudentId,
                ObjectState = ObjectState.Unchanged,
                RowVersion = student.RowVersion
            };

            foreach (Examination exam in student.ExamsTaken)
            {
                var examViewModel = new ExaminationViewModel
                {
                    Subject = exam.Subject,
                    MaximumMarks = exam.MaximumMarks,
                    MarksObtained = exam.MarksObtained,
                    DateTaken = exam.DateTaken.ToString(CultureInfo.CurrentCulture),
                    Guid = exam.Guid,
                    DateCreated = exam.DateCreated.ToString(CultureInfo.CurrentCulture),
                    DateModified = exam.DateModified.ToString(CultureInfo.CurrentCulture),
                    StudentId = student.StudentId,
                    ExaminationId = exam.ExaminationId,
                    RowVersion = exam.RowVersion
                };
                model.ExamsTaken.Add(examViewModel);
            }
            return model;
        }

        public static Student CreateStudentFromStudentViewModel(StudentViewModel studentViewModel)
        {
            var student = new Student
                          {
                              DateOfBirth =
                                  DateTime.Parse(studentViewModel.DateOfBirth, CultureInfo.CurrentCulture),
                              FirstName = studentViewModel.FirstName,
                              LastName = studentViewModel.LastName,
                              Gender = (GenderEnum)studentViewModel.Gender,
                              StudentId = studentViewModel.StudentId,
                              Guid = studentViewModel.Guid,
                              DateCreated = !string.IsNullOrWhiteSpace(studentViewModel.DateCreated) ? DateTime.Parse(studentViewModel.DateCreated, CultureInfo.CurrentCulture) : DateTime.Now,
                              DateModified = !string.IsNullOrWhiteSpace(studentViewModel.DateModified) ? DateTime.Parse(studentViewModel.DateModified, CultureInfo.CurrentCulture) : DateTime.Now,
                              ObjectState = studentViewModel.ObjectState,
                              RowVersion = studentViewModel.RowVersion
                          };

            int temporaryExamId = -1; //m7-10

            foreach (var examinationViewModel in studentViewModel.ExamsTaken)
            {
                var exam = new Examination
                {
                    Subject = examinationViewModel.Subject,
                    MaximumMarks = examinationViewModel.MaximumMarks,
                    MarksObtained = examinationViewModel.MarksObtained,
                    StudentId = examinationViewModel.StudentId,
                    DateTaken = DateTime.Parse(examinationViewModel.DateTaken, CultureInfo.CurrentCulture),
                    Guid = examinationViewModel.Guid,
                    DateCreated = !string.IsNullOrWhiteSpace(examinationViewModel.DateCreated) ? DateTime.Parse(examinationViewModel.DateCreated, CultureInfo.CurrentCulture) : DateTime.Now,
                    DateModified = !string.IsNullOrWhiteSpace(examinationViewModel.DateModified) ? DateTime.Parse(examinationViewModel.DateModified, CultureInfo.CurrentCulture) : DateTime.Now,
                    ObjectState = examinationViewModel.ObjectState,
                    RowVersion = examinationViewModel.RowVersion
                };

                if (examinationViewModel.ObjectState != ObjectState.Added)
                {
                    exam.ExaminationId = examinationViewModel.ExaminationId;
                }
                else
                {
                    exam.ExaminationId = temporaryExamId;
                    temporaryExamId--;
                }
                exam.StudentId = studentViewModel.StudentId;
                student.ExamsTaken.Add(exam);
            }



            return student;
        }

        public static string GetMessageToClient(ObjectState objectState, Student student)
        {
            string messageToClient = string.Empty;

            switch (objectState)
            {
                case ObjectState.Added:
                    messageToClient = string.Format("{0}, {1} has been added to the database.",
                        student.FirstName, student.LastName);
                    break;

                case ObjectState.Modified:
                    messageToClient = string.Format("{0}, {1} has been updated.", student.FirstName,
                        student.LastName);
                    break;
            }

            return messageToClient;
        }
    }
}