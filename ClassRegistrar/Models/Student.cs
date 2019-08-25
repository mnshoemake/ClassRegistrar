using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ClassRegistrar.Models
{
    public class Student
    {
        private int _StudentID;
        private string _StudentName;
        private string _StudentEmail;
        private string _StudentLogin;
        private string _StudentPassword;
        private List<Models.Class> _StudentClasses;

        public Student()
        {
            StudentID = 1000;
            StudentName = "ERROR";
            StudentEmail = "ERROR";
            StudentLogin = "ERROR";
            StudentPassword = "ERROR";
        }

        public Student(int studentID, string studentName, string studentEmail, string studentLogin, string studentPassword)
        {
            this.StudentID = studentID;
            this.StudentName = studentName;
            this.StudentEmail = studentEmail;
            this.StudentLogin = studentLogin;
            this.StudentPassword = studentPassword;
        }

        public Student(int StudentID, string StudentName, string StudentEmail, string StudentLogin, string StudentPassword, List<ClassRegistrar.Processor.Class> StudentClasses)
        {
            List<Models.Class> modelStudentClasses = new List<Models.Class>();
            this.StudentID = StudentID;
            this.StudentName = StudentName;
            this.StudentEmail = StudentEmail;
            this.StudentLogin = StudentLogin;
            this.StudentPassword = StudentPassword;
            foreach (ClassRegistrar.Processor.Class aClass in StudentClasses)
            {
                Models.Class modelClass = new Models.Class(aClass.ClassID, aClass.ClassName, aClass.ClassDate, aClass.ClassDescription);
                modelStudentClasses.Add(modelClass);
            }
            this.StudentClasses = modelStudentClasses;
        }

        
        public int StudentID { get => _StudentID; set => _StudentID = value; }
        public string StudentName { get => _StudentName; set => _StudentName = value; }
        public string StudentEmail { get => _StudentEmail; set => _StudentEmail = value; }
        [Required]
        public string StudentLogin { get => _StudentLogin; set => _StudentLogin = value; }
        [Required]
        public string StudentPassword { get => _StudentPassword; set => _StudentPassword = value; }
        public List<Models.Class> StudentClasses { get => _StudentClasses; set => _StudentClasses = value; }

    }


}