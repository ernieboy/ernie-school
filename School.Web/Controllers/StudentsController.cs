using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using School.DataLayer;
using School.Models;
using School.Web.ViewModels;
using System.Data.Entity.Infrastructure;

namespace School.Web.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private SchoolContext _schoolContext;

        public StudentsController()
        {
            _schoolContext = new SchoolContext();
        }

        // GET: /Students/
        public async Task<ActionResult> Index()
        {
            return View(await _schoolContext.Students.ToListAsync());
        }

        // GET: /Students/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = await _schoolContext.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            var model = School.Web.ViewModels.Helpers.CreateStudentViewModelFromStudent(student);

            return View(model);
        }

        // GET: /Students/Create
        public ActionResult Create()
        {
            var model = new StudentViewModel { ObjectState = ObjectState.Added };
            return View(model);
        }

        [HandleModelStateException]
        public JsonResult Save(StudentViewModel studentViewModel)
        {
            if (!ModelState.IsValid) throw new ModelStateException(ModelState);

            var student = School.Web.ViewModels.Helpers.CreateStudentFromStudentViewModel(studentViewModel);
            SetStudentRecordHistoryDetails(studentViewModel, student);
            _schoolContext.Students.Attach(student);

            InformEfAboutDeletedExaminations(studentViewModel, student);

            _schoolContext.ApplyStateChanges();
            string messageToClient = string.Empty;
            try
            {
                _schoolContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                messageToClient = "Someone else has modified this record in the database since you retrieved it. Your changes have been discarded. What you see now are the current values in the database. ";
            }
            catch (Exception ex)
            {
                throw new ModelStateException(ex);
            }

            if (studentViewModel.ObjectState == ObjectState.Deleted)
            {
                return Json(new { newLocation = "/Students/Index/?deleted=y" });
            }
            if (messageToClient.Trim().Length == 0)
            {
                messageToClient = School.Web.ViewModels.Helpers.GetMessageToClient(studentViewModel.ObjectState, student);
            }

            //Refresh student from the database. this is useful in situations where someone else has edited the record and we want to see the latest record.
            studentViewModel.StudentId = student.StudentId;
            _schoolContext.Dispose();
            _schoolContext = new SchoolContext();
            student = _schoolContext.Students.Find(studentViewModel.StudentId);

            //Refresh the view model from the updated student entity before sending it back to the client.
            studentViewModel = ViewModels.Helpers.CreateStudentViewModelFromStudent(student);
            studentViewModel.MessageToClient = messageToClient;

            return Json(new { studentViewModel }); //Return an anonymous JSON object and let the client inspect it.
        }

        private void InformEfAboutDeletedExaminations(StudentViewModel studentViewModel, Student student)
        {
            if (student.ObjectState == ObjectState.Deleted)
            {
                foreach (Examination exam in student.ExamsTaken)
                {
                    var examination = _schoolContext.Exams.Find(exam.ExaminationId);
                    if (examination != null)
                    {
                        examination.ObjectState = ObjectState.Deleted;
                    }
                }
            }
            else
            {
                foreach (int id in studentViewModel.ExamItemsToDelete)
                {
                    Examination exam = _schoolContext.Exams.Find(id);
                    if (exam != null)
                    {
                        exam.ObjectState = ObjectState.Deleted;
                    }
                }
            }
        }

        private static void SetStudentRecordHistoryDetails(StudentViewModel studentViewModel, Student student)
        {
            if (studentViewModel.ObjectState == ObjectState.Added)
            {
                student.DateCreated = DateTime.Now;
                student.Guid = Guid.NewGuid().ToString("N");
                student.DateModified = DateTime.Now;
            }
            else if (studentViewModel.ObjectState == ObjectState.Modified)
            {
                student.DateModified = DateTime.Now;
                student.DateCreated = DateTime.Parse(studentViewModel.DateCreated, CultureInfo.CurrentCulture);
            }

            foreach (var exam in student.ExamsTaken)
            {
                if (exam.ObjectState == ObjectState.Added)
                {
                    exam.DateCreated = DateTime.Now;
                    exam.Guid = Guid.NewGuid().ToString("N");
                    exam.DateModified = DateTime.Now;
                }
                else if (exam.ObjectState == ObjectState.Modified)
                {
                    exam.DateModified = DateTime.Now;
                    exam.DateCreated = exam.DateCreated;
                }
            }
        }

        // GET: /Students/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = await _schoolContext.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            var model = School.Web.ViewModels.Helpers.CreateStudentViewModelFromStudent(student);

            return View(model);
        }

        // GET: /Students/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = await _schoolContext.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            var model = School.Web.ViewModels.Helpers.CreateStudentViewModelFromStudent(student);
            model.MessageToClient = string.Format("You are about to delete {0}, {1}.", student.FirstName, student.LastName);
            model.ObjectState = ObjectState.Deleted;
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _schoolContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}