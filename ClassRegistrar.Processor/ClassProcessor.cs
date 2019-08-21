using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ClassRegistrar.Processor
{
    public class Class
    {

        private int _ClassID;
        private string _ClassName;
        private DateTime _ClassDate;
        private string _ClassDescription;
        
        //Constructor
        public Class(int ClassID, string ClassName, DateTime ClassDate, string ClassDescription)
        {
            this.ClassID = ClassID;
            this.ClassName = ClassName;
            this.ClassDate = ClassDate;
            this.ClassDescription = ClassDescription;
        }

        public int ClassID { get => _ClassID; set => _ClassID = value; }
        public string ClassName { get => _ClassName; set => _ClassName = value; }
        public DateTime ClassDate { get => _ClassDate; set => _ClassDate = value; }
        public string ClassDescription { get => _ClassDescription; set => _ClassDescription = value; }
        
    }

    public class ClassProcessor
    {
        public List<Class> Select(string ConnectionString)
        {
            try
            {
                List<Class> Classes = new List<Class>();

                string strCmd =
                    @"SELECT 
                        ClassId
                        ,ClassName
                        ,ClassDate
                        ,ClassDescription
                    FROM vClasses;"
                    ;

                SqlConnection sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand sqlCommand = new SqlCommand(strCmd, sqlConnection);
                sqlConnection.Open();


                System.Data.IDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    Class aClass = new Class(
                                                (int)dataReader["ClassId"]
                                                , (string)dataReader["ClassName"]
                                                , (DateTime)dataReader["ClassDate"]
                                                , (string)dataReader["ClassDescription"]
                                                );
                    Classes.Add(aClass);
                }


                sqlConnection.Close();

                return Classes;
            }
            catch (Exception)
            { throw; }
        }

        public int Delete(string ConnectionString, int StudentID)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pDelClasses", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter objP0 = new SqlParameter();
                objP0.Direction = System.Data.ParameterDirection.ReturnValue;
                objP0.ParameterName = "@RC";
                objP0.SqlDbType = System.Data.SqlDbType.Int;
                objCmd.Parameters.Add(objP0);

                SqlParameter objP1 = new SqlParameter();
                objP1.Direction = System.Data.ParameterDirection.Input;
                objP1.ParameterName = "@ClassID";
                objP1.SqlDbType = System.Data.SqlDbType.Int;
                objCmd.Parameters.Add(objP1);
                objCmd.Parameters["@ClassID"].Value = StudentID;

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
            objP1.ParameterName = "@ClassID";
            objP1.SqlDbType = System.Data.SqlDbType.Int;
            objCmd.Parameters.Add(objP1);

            SqlParameter objP2 = new SqlParameter();
            objP2.Direction = System.Data.ParameterDirection.Input;
            objP2.ParameterName = "@ClassName";
            objP2.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP2.Size = 100;
            objCmd.Parameters.Add(objP2);

            SqlParameter objP3 = new SqlParameter();
            objP3.Direction = System.Data.ParameterDirection.Input;
            objP3.ParameterName = "@ClassDate";
            objP3.SqlDbType = System.Data.SqlDbType.DateTime;
            objP3.Size = 100;
            objCmd.Parameters.Add(objP3);

            SqlParameter objP4 = new SqlParameter();
            objP4.Direction = System.Data.ParameterDirection.Input;
            objP4.ParameterName = "@ClassDescription";
            objP4.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP4.Size = 500;
            objCmd.Parameters.Add(objP4);
        }

        public int Insert(string ConnectionString, int ClassID, string ClassName, DateTime ClassDate, string ClassDescription, string StudentPassword)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pInsClasses", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetupParameters(ref objCmd);
                objCmd.Parameters["@ClassID"].Value = ClassID;
                objCmd.Parameters["@ClassName"].Value = ClassName;
                objCmd.Parameters["@ClassDate"].Value = ClassDate;
                objCmd.Parameters["@ClassDescription"].Value = ClassDescription;
                
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

        public int Update(string ConnectionString, int ClassID, string ClassName, DateTime ClassDate, string ClassDescription)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pUpdClasses", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetupParameters(ref objCmd);
                objCmd.Parameters["@ClassID"].Value = ClassID;
                objCmd.Parameters["@ClassName"].Value = ClassName;
                objCmd.Parameters["@ClassDate"].Value = ClassDate;
                objCmd.Parameters["@StudentLogin"].Value = ClassDescription;

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
