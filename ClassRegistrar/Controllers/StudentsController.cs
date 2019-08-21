using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClassRegistrar.Controllers
{
    public class StudentsController : Controller
    {
        private ClassRegistrar.Processor.StudentProcessor studentProcessor;
        private string strConnectionString;

        public StudentsController()
        {
            studentProcessor = new ClassRegistrar.Processor.StudentProcessor();
            strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AdvWebDevProjectConString"].ConnectionString;
        }

        
        private List<Models.Student> SelectAllStudents()
        {
            List<ClassRegistrar.Processor.Student> lstProcessorStudents = studentProcessor.Select(strConnectionString);

            Models.Student objStudent;
            List<Models.Student> lstStudents = new List<Models.Student>();

            foreach (var processorStudent in lstProcessorStudents)
            {
                objStudent = new Models.Student(processorStudent.StudentID,
                    processorStudent.StudentName,
                    processorStudent.StudentEmail,
                    processorStudent.StudentLogin, 
                    processorStudent.StudentPassword);

                lstStudents.Add(objStudent);
            }
            return lstStudents;
        }

        private Models.Student SelectOneStudents(int id)
        {
            var student = new object();
            foreach (var item in SelectAllStudents())
            {
                if (item.StudentID == id) student = item;
            }
            return (Models.Student)student;
        }


        #region SelectAll
        public ActionResult Index()
        {
            return View(SelectAllStudents());
        }
        #endregion


        #region SelectOne
        public ActionResult Details(int id)
        {
            return View(SelectOneStudents(id));
        }
        #endregion


        #region Create
        public ActionResult Create() 
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection) 
        {
            ViewData["Error"] = ""; 
            try
            {
                studentProcessor.Insert(strConnectionString
                                  , int.Parse(collection["StudentID"])
                                  , collection["StudentName"]
                                  , collection["StudentEmail"]
                                  , collection["StudentLogin"]
                                  , collection["StudentPassword"]);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
        #endregion


        #region Edit
        public ActionResult Edit(int id)
        {
            return View(SelectOneStudents(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            ViewData["Error"] = ""; //You must declare this here, or it's "Conditonally" created in the Catch block
            try
            {

                // TODO: Add Insert logic here
                studentProcessor.Update(strConnectionString
                                  , int.Parse(collection["StudentID"])
                                  , collection["StudentName"]
                                  , collection["StudentEmail"]
                                  , collection["StudentLogin"]
                                  , collection["StudentPassword"]);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
        #endregion

        
        #region Delete
        public ActionResult Delete(int id)
        {
            return View(SelectOneStudents(id));
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ViewData["Error"] = "";
            try
            {
                studentProcessor.Delete(strConnectionString, id);
                ViewData["StudentID"] = id;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
        #endregion
    }
}