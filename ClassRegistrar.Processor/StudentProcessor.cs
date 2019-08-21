using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ClassRegistrar.Processor
{
    public class Student
    {
        //Can be replaced with Invisible Auto Property fields
        private int _StudentID;
        private string _StudentName;
        private string _StudentEmail;
        private string _StudentLogin;
        private string _StudentPassword;
        private List<Class> _StudentClasses;

        //Constructor
        public Student(int StudentID, string StudentName, string StudentEmail, string StudentLogin, string StudentPassword)
        {
            this.StudentID = StudentID;
            this.StudentName = StudentName;
            this.StudentEmail = StudentEmail;
            this.StudentLogin = StudentLogin;
            this.StudentPassword = StudentPassword;
        }

        public Student(int StudentID, string StudentName, string StudentEmail, string StudentLogin, string StudentPassword, List<Class> StudentClasses)
        {
            this.StudentID = StudentID;
            this.StudentName = StudentName;
            this.StudentEmail = StudentEmail;
            this.StudentLogin = StudentLogin;
            this.StudentPassword = StudentPassword;
            this.StudentClasses = StudentClasses;
        }

        public int StudentID { get => _StudentID; set => _StudentID = value; }
        public string StudentName { get => _StudentName; set => _StudentName = value; }
        public string StudentEmail { get => _StudentEmail; set => _StudentEmail = value; }
        public string StudentLogin { get => _StudentLogin; set => _StudentLogin = value; }
        public string StudentPassword { get => _StudentPassword; set => _StudentPassword = value; }
        public List<Class> StudentClasses { get => _StudentClasses; set => _StudentClasses = value; }

        public override string ToString()
        {
            return this.StudentID + ","
                + StudentName + ","
                + StudentEmail + ","
                + StudentLogin + ","
                + StudentPassword + ";";
        }
    }

    public class StudentProcessor
    {
        public List<Student> Select(string ConnectionString)
        {
            try
            {
                List<Student> Students = new List<Student>();
                
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    string strCmd =
                        @"SELECT 
                            StudentID
                            ,StudentName
                            ,StudentEmail
                            ,StudentLogin
                            ,StudentPassword 
                        FROM vStudents;";

                    using (SqlCommand sqlCommand = new SqlCommand(strCmd, sqlConnection))
                    {
                        System.Data.IDataReader dataReader = sqlCommand.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Student student = new Student(
                                                        (int)dataReader["StudentID"]
                                                        , (string)dataReader["StudentName"]
                                                        , (string)dataReader["StudentEmail"]
                                                        , (string)dataReader["StudentLogin"]
                                                        , (string)dataReader["StudentPassword"]
                                                        );
                            Students.Add(student);
                        }
                        dataReader.Close();
                        dataReader.Dispose();
                    }


                    foreach (Student student in Students)
                    {
                        List<Class> Classes = new List<Class>();
                        strCmd = "pSelClassesByStudentId";
                        
                        using (SqlCommand getClassesForStudent = new SqlCommand(strCmd, sqlConnection))
                        {
                            
                            getClassesForStudent.CommandType = System.Data.CommandType.StoredProcedure;

                            SqlParameter objP0 = new SqlParameter();
                            objP0.Direction = System.Data.ParameterDirection.ReturnValue;
                            objP0.ParameterName = "@RC";
                            objP0.SqlDbType = System.Data.SqlDbType.Int;
                            getClassesForStudent.Parameters.Add(objP0);

                            SqlParameter objP1 = new SqlParameter();
                            objP1.Direction = System.Data.ParameterDirection.Input;
                            objP1.ParameterName = "@StudentID";
                            objP1.SqlDbType = System.Data.SqlDbType.Int;
                            getClassesForStudent.Parameters.Add(objP1);

                            getClassesForStudent.Parameters["@StudentID"].Value = student.StudentID;

                            System.Data.IDataReader dataReader2 = getClassesForStudent.ExecuteReader();

                            while (dataReader2.Read())
                            {
                                Class aClass = new Class(
                                    (int)dataReader2["ClassID"]
                                    , (string)dataReader2["ClassName"]
                                    , (DateTime)dataReader2["ClassDate"]
                                    , (string)dataReader2["ClassDescription"]
                                    );
                                Classes.Add(aClass);
                            }

                            dataReader2.Close();
                            dataReader2.Dispose();

                        }
                        student.StudentClasses = Classes;
                    }

                }
                return Students;
            }
            catch (Exception)
            { throw; }
        }

        public int Delete(string ConnectionString, int StudentID)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pDelStudents", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter objP0 = new SqlParameter();
                objP0.Direction = System.Data.ParameterDirection.ReturnValue;
                objP0.ParameterName = "@RC";
                objP0.SqlDbType = System.Data.SqlDbType.Int;
                objCmd.Parameters.Add(objP0);

                SqlParameter objP1 = new SqlParameter();
                objP1.Direction = System.Data.ParameterDirection.Input;
                objP1.ParameterName = "@StudentID";
                objP1.SqlDbType = System.Data.SqlDbType.Int;
                objCmd.Parameters.Add(objP1);
                objCmd.Parameters["@StudentID"].Value = StudentID;

                try
                {
                    objCon.Open();
                    objCmd.ExecuteNonQuery();
                    if ((int)objCmd.Parameters["@RC"].Value < 0) { throw (new Exception("An internal problem was reported by the stored procedure: " + objCmd.Parameters["@RC"].Value.ToString())); }
                }
                catch { throw; }
                finally { objCon.Close(); }
                return (int)objCmd.Parameters["@RC"].Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void SetupParameters(ref SqlCommand objCmd)
        {
            SqlParameter objP0 = new SqlParameter();
            objP0.Direction = System.Data.ParameterDirection.ReturnValue;
            objP0.ParameterName = "@RC";
            objP0.SqlDbType = System.Data.SqlDbType.Int;
            objCmd.Parameters.Add(objP0);

            SqlParameter objP1 = new SqlParameter();
            objP1.Direction = System.Data.ParameterDirection.Input;
            objP1.ParameterName = "@StudentID";
            objP1.SqlDbType = System.Data.SqlDbType.Int;
            objCmd.Parameters.Add(objP1);

            SqlParameter objP2 = new SqlParameter();
            objP2.Direction = System.Data.ParameterDirection.Input;
            objP2.ParameterName = "@StudentName";
            objP2.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP2.Size = 100;
            objCmd.Parameters.Add(objP2);

            SqlParameter objP3 = new SqlParameter();
            objP3.Direction = System.Data.ParameterDirection.Input;
            objP3.ParameterName = "@StudentEmail";
            objP3.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP3.Size = 100;
            objCmd.Parameters.Add(objP3);

            SqlParameter objP4 = new SqlParameter();
            objP4.Direction = System.Data.ParameterDirection.Input;
            objP4.ParameterName = "@StudentLogin";
            objP4.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP4.Size = 50;
            objCmd.Parameters.Add(objP4);

            SqlParameter objP5 = new SqlParameter();
            objP5.Direction = System.Data.ParameterDirection.Input;
            objP5.ParameterName = "@StudentPassword";
            objP5.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP5.Size = 50;
            objCmd.Parameters.Add(objP5);
        }

        public int Insert(string ConnectionString, int StudentID, string StudentName, string StudentEmail, string StudentLogin, string StudentPassword)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pInsStudents", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetupParameters(ref objCmd);
                objCmd.Parameters["@StudentID"].Value = StudentID;
                objCmd.Parameters["@StudentName"].Value = StudentName;
                objCmd.Parameters["@StudentEmail"].Value = StudentEmail;
                objCmd.Parameters["@StudentLogin"].Value = StudentLogin;
                objCmd.Parameters["@StudentPassword"].Value = StudentPassword;
                try
                {
                    objCon.Open();
                    objCmd.ExecuteNonQuery();
                    if ((int)objCmd.Parameters["@RC"].Value < 0) { throw (new Exception("An internal problem was reported by the stored procedure: " + objCmd.Parameters["@RC"].Value.ToString())); }
                }
                catch { throw; }
                finally { objCon.Close(); }
                return (int)objCmd.Parameters["@RC"].Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(string ConnectionString, int StudentID, string StudentName, string StudentEmail, string StudentLogin, string StudentPassword)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pUpdStudents", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetupParameters(ref objCmd);
                objCmd.Parameters["@StudentID"].Value = StudentID;
                objCmd.Parameters["@StudentName"].Value = StudentName;
                objCmd.Parameters["@StudentEmail"].Value = StudentEmail;
                objCmd.Parameters["@StudentLogin"].Value = StudentLogin;
                objCmd.Parameters["@StudentPassword"].Value = StudentPassword;
                try
                {
                    objCon.Open();
                    objCmd.ExecuteNonQuery();
                    if ((int)objCmd.Parameters["@RC"].Value < 0) { throw (new Exception("An internal problem was reported by the stored procedure: " + objCmd.Parameters["@RC"].Value.ToString())); }
                }
                catch { throw; }
                finally { objCon.Close(); }
                return (int)objCmd.Parameters["@RC"].Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

       



    }
}
