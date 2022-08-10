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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeController : ApiController
    {
        //declaring variables
        SqlConnection conn;
        SqlCommand cmd;
        Employee empObj = new Employee();
        Log logObj = new Log();
        
        /// <summary>
        /// This will validate the user and if exist then it will enter the email and time of login
        /// </summary>
        /// <param name="empObj"></param>
        /// <returns></returns>
        #region Entry Login
        [HttpPost]
        [ActionName("ValidateEmp")]
        public bool PostValidation(Employee empObj)
        {
            try
            {
                //validating the user is present or not
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery = "select count(*) as count from Employee where vemail='" + empObj.vemail
                    + "' and nmob_number=" + empObj.nmob_number;
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
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        #endregion

        /// <summary>
        /// Here the employee can get the name of the employee who are in their department
        /// </summary>
        /// <param name="empObj"></param>
        /// <returns></returns>
        #region Get Team Mate Name
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
                SqlCommand cmd = new SqlCommand("select vFirst_name from Employee where vDepartment='"+empObj.vDepartment+"'", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        empObj.vFirst_name = (Convert.ToString(dt.Rows[i]["vFirst_name"]));
                        
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
        /// Here the employee can update his/her own detail
        /// </summary>
        /// <param name="empObj"></param>
        #region Update Employee
        [HttpPut]
        [ActionName("UpdateEmp")]
        public void putEmployee([FromBody]Employee empObj)
        {
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery = "update Employee set vfirst_name='" + empObj.vFirst_name + "'," +
                    "vMiddle_name='" + empObj.vMiddle_name + "'," + "vLast_name='" + empObj.vLast_name + "',vemail='" +
                    empObj.vemail + "'," + "nmob_number=" + empObj.nmob_number + "," + "cGender='" + empObj.cGender
                    + "'," + "dDOB='" + empObj.dDOB + "'," + "dDOJ='" + empObj.dDOJ + "'," + "vdepartment='" +
                    empObj.vDepartment + "'," + "mSalary=" + empObj.mSalary + "," + "vAddress='" + empObj.vAddress + "'" +
                    "where vemail='" + empObj.vemail + "'";
                SqlCommand cmd = new SqlCommand(sQuery, conn);
                int update = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        /// <summary>
        /// This will keep record of the log out and log in detail of Employee
        /// </summary>
        /// <param name="logObj"></param>
        /// <returns></returns>
        #region Updating the check_log table 
        [HttpPut]
        [ActionName("Logout")]
        public Log Logout(Log logObj)
        {
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery3 = "update check_log set dtlogout='" + DateTime.Now + "'" + "where vemail='" + logObj.vemail + "'";
                cmd = new SqlCommand(sQuery3, conn);
                cmd.ExecuteNonQuery();
                return logObj;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
