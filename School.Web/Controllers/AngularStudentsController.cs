using Newtonsoft.Json;
using School.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace School.Web.Controllers
{
     [Authorize]
    public class AngularStudentsController : Controller
    {
        private SchoolContext _schoolContext;

        public AngularStudentsController()
        {
            _schoolContext = new SchoolContext();
        }

        //
        // GET: /AngularStudents/
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult AllStudents()
        {
            var list = _schoolContext.Students.ToList();
            string json = JsonConvert.SerializeObject(list);
            return Json(new { json }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Students/Details/5
        public async Task<JsonResult> FindById(int? id)
        {
            if (id == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
            var student = await _schoolContext.Students.FindAsync(id);
            if (student == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.NotFound));
            }
            string json = JsonConvert.SerializeObject(student);
            return Json(new { json }, JsonRequestBehavior.AllowGet);
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