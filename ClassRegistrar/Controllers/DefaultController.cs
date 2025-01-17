﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;

namespace ClassRegistrar.Controllers
{
    public class DefaultController : Controller
    {
        ClassRegistrar.Processor.StudentProcessor studentProcessor = new ClassRegistrar.Processor.StudentProcessor();
        ClassRegistrar.Processor.LoginRequestProcessor loginRequestProcessor = new ClassRegistrar.Processor.LoginRequestProcessor();
        ClassRegistrar.Processor.ClassProcessor classProcessor = new ClassRegistrar.Processor.ClassProcessor();
        ClassRegistrar.Processor.ClassStudentProcessor classStudentProcessor = new ClassRegistrar.Processor.ClassStudentProcessor();
        string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AdvWebDevProjectConString"].ConnectionString;


        #region StudentSelect
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
                    processorStudent.StudentPassword,
                    processorStudent.StudentClasses);

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
        #endregion

        #region LoginTools
        private int GetNewLoginID()
        {
            List<ClassRegistrar.Processor.LoginRequest> lstProcessorLoginRequests = loginRequestProcessor.Select(strConnectionString);

            int highestID = 0;

            foreach (var processorLoginRequest in lstProcessorLoginRequests)
            {
                if (processorLoginRequest.LoginID > highestID)
                {
                    highestID = processorLoginRequest.LoginID;
                }
            }
            highestID++;
            return highestID;
        }
        #endregion



        private List<Models.Class> SelectAllClasses()
        {
            List<ClassRegistrar.Processor.Class> lstProcessorClasses = classProcessor.Select(strConnectionString);

            Models.Class objClass;
            List<Models.Class> lstClasses = new List<Models.Class>();

            foreach (var processorClass in lstProcessorClasses)
            {
                objClass = new Models.Class(processorClass.ClassID,
                    processorClass.ClassName,
                    processorClass.ClassDate,
                    processorClass.ClassDescription
                    );

                lstClasses.Add(objClass);
            }
            return lstClasses;
        }

        private List<Models.Class> SelectClassesByStudent(int studentID)
        {
            List<Models.Student> lstStudents = SelectAllStudents();
            var result = lstStudents.Select(student => student).Where(student => student.StudentID == studentID);
            Models.Student studentResult = result.First();
            
            return studentResult.StudentClasses;
        }


        #region Login
        public ActionResult Login()
        {
            if (Session["StudentID"] != null)
            {
                int studentID = int.Parse(Session["StudentID"].ToString());
                return View(SelectOneStudents(studentID));
            }
            else return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon(); 
            return RedirectToAction("Login", "Default");
        }
        

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            ViewData["Error"] = "";
            try
            {
                //1.Declare list of processor students. Use Processor to get list of students
                List<ClassRegistrar.Processor.Student> lstProcessorStudents = studentProcessor.Select(strConnectionString);

                //2. Declare Model student object to move information from Processor to Model to list of Models.
                Models.Student modelStudent;

                //3. Declare list of Model students.
                List<Models.Student> lstStudents = new List<Models.Student>();

                //4. Go through the list returned by the processor, map the information to a Model student, add the Model student to the list of Model students.

                foreach (var processorStudent in lstProcessorStudents)
                {
                    modelStudent = new Models.Student(processorStudent.StudentID,
                        processorStudent.StudentName,
                        processorStudent.StudentEmail,
                        processorStudent.StudentLogin,
                        processorStudent.StudentPassword);

                    lstStudents.Add(modelStudent);
                }

                var result =
                        lstStudents.Select(student => student)
                            .Where(student =>
                                student.StudentLogin == collection["StudentLogin"]
                                && student.StudentPassword == collection["StudentPassword"]
                                );

                var studentResult = result.First();

                if (result.Count() > 0)
                {
                    Session["StudentID"] = studentResult.StudentID;
                    return RedirectToAction("MyClasses");
                }
                return View();
            }
            catch (Exception)
            {
                ViewData["Error"] = "Invalid Login";//ex.Message;
                return View();
            }
        }
        #endregion

        #region NewLogin
       
        public ActionResult NewLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewLogin(Models.LoginRequest login)
        
        {
            
            login.LoginID = GetNewLoginID();

            if (ModelState.IsValid)
            {
                ViewData["Error"] = "";
                try
                {
                    loginRequestProcessor.Insert(strConnectionString
                        , login.LoginID
                        , login.Name
                        , login.EmailAddress
                        , login.LoginName
                        , login.NewOrReactivate
                        , login.ReasonForAccess
                        , login.DateRequiredBy);
                    ViewData["SubmitLoginSuccess"] = true;
                    return View(login);
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = ex.Message;
                    return View(login);
                }
            }

            return View(login);
        }
        #endregion NewLogin

        #region Classes
        public ActionResult Classes()
        {
            return View(SelectAllClasses());
        }
        #endregion 

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        




        public ActionResult Register()
        {
            if (Session["StudentID"] != null)
            {
                int studentID = int.Parse(Session["StudentID"].ToString());
                var classesStudentIsRegisteredFor = SelectClassesByStudent(studentID);
                var availableClasses = SelectAllClasses();

                List<int> lstClassIDsStudentIsCurrentlyRegisteredFor
                    = new List<int>(classesStudentIsRegisteredFor.Select(x => x.ClassID));

                availableClasses.RemoveAll(x => lstClassIDsStudentIsCurrentlyRegisteredFor.Contains(x.ClassID));

                return View(availableClasses);
            }
            else { return RedirectToAction("Login"); }


        }

        [HttpPost]
        public ActionResult Register(int id)
        {
            if (Session["StudentID"] != null)
            {
                int studentID = int.Parse(Session["StudentID"].ToString());
                classStudentProcessor.Insert(strConnectionString, studentID, id);
                var classesStudentIsRegisteredFor = SelectClassesByStudent(studentID);
                var availableClasses = SelectAllClasses();

                List<int> lstClassIDsStudentIsCurrentlyRegisteredFor
                    = new List<int>(classesStudentIsRegisteredFor.Select(x => x.ClassID));

                availableClasses.RemoveAll(x => lstClassIDsStudentIsCurrentlyRegisteredFor.Contains(x.ClassID));

                return View(availableClasses);
            }
            else { return RedirectToAction("Login"); }
            
        }


        public ActionResult MyClasses()
        {
            if (Session["StudentID"] != null)
            {
                int studentID = int.Parse(Session["StudentID"].ToString());
                return View(SelectClassesByStudent(studentID));
            }
            else { return RedirectToAction("Login"); }

        }


    }
}