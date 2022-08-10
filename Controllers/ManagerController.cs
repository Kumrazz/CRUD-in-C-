using ManufacturerAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ManufacturerAPI.Controllers
{
    /// <summary>
    /// Controller for Manager role
    /// </summary>
    /// 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ManagerController : ApiController
    {
        //declaring variables
        SqlConnection conn;
        SqlCommand cmd;
        Employee empObj = new Employee();

        /// <summary>
        /// This will validate the user and if exist then it will enter the email and time of login
        /// </summary>
        /// <param name="empObj"></param>
        /// <returns></returns>
        #region Entry Login
        [HttpPost]
        [ActionName("ValidateMan")]
        public string PostValidation(Employee empObj)
        {
            try
            {
                //validating the user is present or not
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery = "select count(*) as count from Employee where vemail='" + empObj.vemail
                    + "' and nmob_number=" + empObj.nmob_number+"and iManId=0";
                SqlCommand cmd = new SqlCommand(sQuery, conn);
                int select = int.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
                if (select > 0)
                {
                    conn.Open();
                    string sQuery2 = "insert into check_log (vemail,dtlogin) values('" + empObj.vemail + "','" + DateTime.Now + "')";
                    SqlCommand cmd2 = new SqlCommand(sQuery2, conn);
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                    return "Sucessfully log in";
                }
                else
                {
                    return "Not a valid user";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        #endregion

        /// <summary>
        /// This is used to create the task and assign the task to employee by Manager
        /// </summary>
        /// <param name="taskObj"></param>
        /// <returns></returns>
        #region AddTask
        Task taskObj = new Task();
        [HttpPost]
        [ActionName("AddTask")]
        public Task addTask(Task taskObj)
        {
            try
            {
                //validating the user is present or not
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery = "insert into task values(" + taskObj.iempid + ",'" + taskObj.vtask_name + "','" + taskObj.vstate + "')";
                SqlCommand cmd = new SqlCommand(sQuery, conn);
                int insert=cmd.ExecuteNonQuery();
                if (insert > 0)
                {
                    return taskObj;
                }
                else
                    return null;
                conn.Close();
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region
        [HttpGet]
        [ActionName("getTask")]
        public List<Task> getTask(Task taskObj)
        {
            string functionName = "GetTask(Man)";
            try
            {
                List<Task> TaskList = new List<Task>();
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Task where iempid='"+taskObj.iempid+"'", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count-1; i++)
                    {

                        taskObj.iempid = Convert.ToInt32(dt.Rows[i]["iempid"]);
                        taskObj.vtask_name = (dt.Rows[i]["vtask_name"].ToString());
                        taskObj.vstate = (dt.Rows[i]["vstate"].ToString());
                        TaskList.Add(taskObj);
                    }
                }
                return TaskList;
            }
            catch (Exception e)
            {
                throw new Exception(functionName + " " + e.Message);
            }
            finally { conn.Close(); }
        }
        #endregion

        /// <summary>
        /// Here the Manager can get the details of the employee who are in their department
        /// </summary>
        /// <param name="empObj"></param>
        /// <returns></returns>
        #region Get Employee Detail
        [HttpGet]
        [ActionName("getEmp")]
        public List<Employee> getEmployee(Employee empObj)
        {
            string functionName = "GetEmployee";
            try
            {
                List<Employee> employeeList = new List<Employee>();
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Employee where imanid='" + empObj.iManId + "'", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        empObj.vFirst_name = (Convert.ToString(dt.Rows[i]["vFirst_name"]));
                        empObj.vMiddle_name = (Convert.ToString(dt.Rows[i]["vMiddle_name"]));
                        empObj.vLast_name = (Convert.ToString(dt.Rows[i]["vLast_name"]));
                        empObj.vPosition = (Convert.ToString(dt.Rows[i]["vPosition"]));
                        empObj.vemail = (Convert.ToString(dt.Rows[i]["vemail"]));
                        empObj.vDepartment = (Convert.ToString(dt.Rows[i]["vDepartment"]));
                        empObj.nmob_number = (Convert.ToInt64(dt.Rows[i]["nmob_number"]));
                        empObj.mSalary = (Convert.ToInt32(dt.Rows[i]["msalary"]));
                        empObj.dDOB = (Convert.ToDateTime(dt.Rows[i]["dDOB"]));
                        empObj.dDOJ = (Convert.ToDateTime(dt.Rows[i]["dDOJ"]));
                        empObj.cGender = (Convert.ToChar(dt.Rows[i]["cGender"]));
                        empObj.vAddress = (Convert.ToString(dt.Rows[i]["vAddress"]));
                        empObj.iManId = (Convert.ToInt32(dt.Rows[i]["iManId"]));
                        employeeList.Add(empObj);
                    }
                }

                conn.Close();
                return employeeList;
            }
            catch (Exception e)
            {
                throw new Exception(functionName + " " + e.Message);
            }
        }
        #endregion

        /// <summary>
        /// This will keep record of the log out and log in detail of Manager
        /// </summary>
        /// <param name="logObj"></param>
        /// <returns></returns>
        #region Updating the check_log table 
        [HttpPut]
        [ActionName("Logout")]
        public string Logout(Log logObj)
        {
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery3 = "update check_log set dtlogout='" + DateTime.Now + "'" + "where vemail='" + logObj.vemail + "'";
                cmd = new SqlCommand(sQuery3, conn);
                cmd.ExecuteNonQuery();
                
                logObj.dtlogout = DateTime.Now;
                return "sucessfully logout";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
