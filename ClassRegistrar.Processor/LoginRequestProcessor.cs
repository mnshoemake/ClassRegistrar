using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ClassRegistrar.Processor
{
    public class LoginRequest
    {
        private int _LoginID;
        private string _Name;
        private string _EmailAddress;
        private string _LoginName;
        private string _NewOrReactivate;
        private string _ReasonForAccess;
        private DateTime _DateRequiredBy;

        public LoginRequest(int loginID
            , string name
            , string emailAddress
            , string loginName
            , string newOrReactivate
            , string reasonForAccess
            , DateTime dateRequiredBy)
        {
            this.LoginID = loginID;
            this.Name = name;
            this.EmailAddress = emailAddress;
            this.LoginName = loginName;
            this.NewOrReactivate = newOrReactivate;
            this.ReasonForAccess = reasonForAccess;
            this.DateRequiredBy = dateRequiredBy;
        }

        public int LoginID { get => _LoginID; set => _LoginID = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string EmailAddress { get => _EmailAddress; set => _EmailAddress = value; }
        public string LoginName { get => _LoginName; set => _LoginName = value; }
        public string NewOrReactivate { get => _NewOrReactivate; set => _NewOrReactivate = value; }
        public string ReasonForAccess { get => _ReasonForAccess; set => _ReasonForAccess = value; }
        public DateTime DateRequiredBy { get => _DateRequiredBy; set => _DateRequiredBy = value; }

    }

    public class LoginRequestProcessor
    {
        public List<LoginRequest> Select(string ConnectionString)
        {
            try
            {
                List<LoginRequest> LoginRequests = new List<LoginRequest>();

                string strCmd =
                    @"SELECT LoginId
                        ,Name
                        ,EmailAddress
                        ,LoginName
                        ,NewOrReactivate
                        ,ReasonForAccess
                        ,DateRequiredBy
                     FROM vLoginRequests;"
                    ;

                SqlConnection sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand sqlCommand = new SqlCommand(strCmd, sqlConnection);
                sqlConnection.Open();


                System.Data.IDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    LoginRequest loginRequest = new LoginRequest(
                                                (int)dataReader["LoginId"]
                                                , (string)dataReader["Name"]
                                                , (string)dataReader["EmailAddress"]
                                                , (string)dataReader["LoginName"]
                                                , (string)dataReader["NewOrReactivate"]
                                                , (string)dataReader["ReasonForAccess"]
                                                , (DateTime)dataReader["DateRequiredBy"]
                                                );
                    LoginRequests.Add(loginRequest);
                }


                sqlConnection.Close();

                return LoginRequests;
            }
            catch (Exception)
            { throw; }
        }

        public int Delete(string ConnectionString, int LoginID)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pDelLoginRequests", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter objP0 = new SqlParameter();
                objP0.Direction = System.Data.ParameterDirection.ReturnValue;
                objP0.ParameterName = "@RC";
                objP0.SqlDbType = System.Data.SqlDbType.Int;
                objCmd.Parameters.Add(objP0);

                SqlParameter objP1 = new SqlParameter();
                objP1.Direction = System.Data.ParameterDirection.Input;
                objP1.ParameterName = "@LoginID";
                objP1.SqlDbType = System.Data.SqlDbType.Int;
                objCmd.Parameters.Add(objP1);
                objCmd.Parameters["@LoginID"].Value = LoginID;

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
            objP1.ParameterName = "@LoginID";
            objP1.SqlDbType = System.Data.SqlDbType.Int;
            objCmd.Parameters.Add(objP1);

            SqlParameter objP2 = new SqlParameter();
            objP2.Direction = System.Data.ParameterDirection.Input;
            objP2.ParameterName = "@Name";
            objP2.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP2.Size = 100;
            objCmd.Parameters.Add(objP2);

            SqlParameter objP3 = new SqlParameter();
            objP3.Direction = System.Data.ParameterDirection.Input;
            objP3.ParameterName = "@EmailAddress";
            objP3.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP3.Size = 100;
            objCmd.Parameters.Add(objP3);

            SqlParameter objP4 = new SqlParameter();
            objP4.Direction = System.Data.ParameterDirection.Input;
            objP4.ParameterName = "@LoginName";
            objP4.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP4.Size = 50;
            objCmd.Parameters.Add(objP4);

            SqlParameter objP5 = new SqlParameter();
            objP5.Direction = System.Data.ParameterDirection.Input;
            objP5.ParameterName = "@NewOrReactivate";
            objP5.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP5.Size = 50;
            objCmd.Parameters.Add(objP5);

            SqlParameter objP6 = new SqlParameter();
            objP6.Direction = System.Data.ParameterDirection.Input;
            objP6.ParameterName = "@ReasonForAccess";
            objP6.SqlDbType = System.Data.SqlDbType.NVarChar;
            objP6.Size = 500;
            objCmd.Parameters.Add(objP6);


            SqlParameter objP7 = new SqlParameter();
            objP7.Direction = System.Data.ParameterDirection.Input;
            objP7.ParameterName = "@DateRequiredBy";
            objP7.SqlDbType = System.Data.SqlDbType.DateTime;
            objP7.Size = 50;
            objCmd.Parameters.Add(objP7);
        }

        public int Insert(string ConnectionString, int loginID, string name, string emailAddress, string loginName, string newOrReactivate, string reasonForAccess, DateTime dateRequiredBy)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pInsLoginRequests", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetupParameters(ref objCmd);
                objCmd.Parameters["@LoginID"].Value = loginID;
                objCmd.Parameters["@Name"].Value = name;
                objCmd.Parameters["@EmailAddress"].Value = emailAddress;
                objCmd.Parameters["@LoginName"].Value = loginName;
                objCmd.Parameters["@NewOrReactivate"].Value = newOrReactivate;
                objCmd.Parameters["@ReasonForAccess"].Value = reasonForAccess;
                objCmd.Parameters["@DateRequiredBy"].Value = dateRequiredBy;
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

        public int Update(string ConnectionString, int loginID, string name, string emailAddress, string loginName, string newOrReactivate, string reasonForAccess, DateTime dateRequiredBy)
        {
            try
            {
                SqlConnection objCon = new SqlConnection(ConnectionString);
                SqlCommand objCmd = new SqlCommand("pUpdLoginRequests", objCon);
                objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetupParameters(ref objCmd);
                objCmd.Parameters["@LoginID"].Value = loginID;
                objCmd.Parameters["@Name"].Value = name;
                objCmd.Parameters["@EmailAddress"].Value = emailAddress;
                objCmd.Parameters["@LoginName"].Value = loginName;
                objCmd.Parameters["@NewOrReactivate"].Value = newOrReactivate;
                objCmd.Parameters["ReasonForAccess"].Value = reasonForAccess;
                objCmd.Parameters["ReasonForAccess"].Value = dateRequiredBy;
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
