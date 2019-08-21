using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ClassRegistrar.Processor
{
    public class ClassStudent
    {
        private int _StudentID;
        private int _ClassID;
        //private string _ClassName;
        //private DateTime _ClassDate;
        //private string _ClassDescription;

        //Constructor
        public ClassStudent(int StudentID, int ClassID, string ClassName, DateTime ClassDate, string ClassDescription)
        {
            this.StudentID = StudentID;
            this.ClassID = ClassID;
            //this.ClassName = ClassName;
            //this.ClassDate = ClassDate;
            //this.ClassDescription = ClassDescription;
        }

        public int StudentID { get => _StudentID; set => _StudentID = value; }
        public int ClassID { get => _ClassID; set => _ClassID = value; }
        //public string ClassName { get => _ClassName; set => _ClassName = value; }
        //public DateTime ClassDate { get => _ClassDate; set => _ClassDate = value; }
        //public string ClassDescription { get => _ClassDescription; set => _ClassDescription = value; }
    }


    public class ClassStudentProcessor
    {

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
            objP2.ParameterName = "@ClassID";
            objP1.SqlDbType = System.Data.SqlDbType.Int;
            objCmd.Parameters.Add(objP2);

        }
        public int Insert(string ConnectionString, int StudentID, int ClassID)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pInsClassStudents", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetupParameters(ref objCmd);
                objCmd.Parameters["@StudentID"].Value = StudentID;
                objCmd.Parameters["@ClassID"].Value = ClassID;

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