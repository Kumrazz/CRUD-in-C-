using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManufacturerAPI.Models;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ManufacturerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminController : ApiController
    {
        #region
        //declaring variables
        SqlConnection conn = new SqlConnection();
        Employee empObj = new Employee();
        /// <summary>
        /// This is for Admin section which can add,update and remove the employee
        /// </summary>
        /// <param name="empObj"></param>
        /// <returns></returns>
        /// 
        #endregion

        /// <summary>
        /// This is for Inserting the new employee in the table
        /// </summary>
        /// <param name="empObj"></param>
        /// <returns></returns>
        /// 
        #region Insertion
        //Adding the Employee
        [HttpPost]
        [ActionName("AddEmployee")]
        public List<Employee> PostEmployee(Employee empObj)
        {

            string functionName = "PostEmployee";
            List<Employee> employeeList = new List<Employee>();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery = "insert into Employee values('" + empObj.vFirst_name + "','" + empObj.vMiddle_name + "','"
                + empObj.vLast_name + "','" + empObj.vemail + "'," + empObj.nmob_number + ",'" +
                empObj.vPosition + "','" + empObj.cGender + "','" + empObj.dDOB + "','" + empObj.dDOJ + "','" +
                empObj.vDepartment + "','" + empObj.vAddress + "'," + empObj.iManId + "," + empObj.mSalary + ")";
                SqlCommand cmd = new SqlCommand(sQuery, conn);
                cmd.ExecuteNonQuery();

                employeeList.Add(empObj);
                return employeeList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { conn.Close(); }


        }
        #endregion

        /// <summary>
        /// This is use for Updating the details of existing Employee
        /// </summary>
        /// <param name="empObj"></param>
        #region Updatation
        // Updating an existing Employee
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
        /// This will use for getting the details of the employee
        /// </summary>
        /// <returns></returns>
        #region Retrieval
        // Retrieving an existing Employee data
        [HttpGet]
        [ActionName("GetEmp")]
        public List<Employee> getEmp()
        {
            string functionName = "GetEmployee";
            try
            {
                List<Employee> employeeList = new List<Employee>();
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Employee where not vfirst_name is null ", conn);
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
                return employeeList;
            }
            catch (Exception e)
            {
                throw new Exception(functionName + " " + e.Message);
            }
            finally { conn.Close(); }

        }
        #endregion

        /// <summary>
        /// This will use to delete the records of existing employee
        /// </summary>
        /// <param name="empObj"></param>
        /// <returns></returns>
        #region Deletion
        [HttpDelete]
        [ActionName("DeleteEmp")]
        public List<Employee> Delemp(Employee empObj)
        {
            string functionName = "Delete function";
            try
            {

                List<Employee> employeeList = new List<Employee>();
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                SqlConnection conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery = "delete from Employee where vemail='" + empObj.vemail + "'";
                SqlCommand cmd = new SqlCommand(sQuery, conn);
                int delete = cmd.ExecuteNonQuery();
                employeeList.Add(empObj);
                
                return employeeList;
            }
            catch (Exception e)
            {
                throw new Exception(functionName + " " + e.Message);
            }
            finally { conn.Close(); }
            
        }
        #endregion

        /// <summary>
        /// This is use for inserting the department and related projects 
        /// </summary>
        /// <param name="deptObj"></param>
        /// <returns></returns>
        #region Add Department
        Department deptObj = new Department();
        [HttpPost]
        [ActionName("AddDept")]
        public List<Department> AddDept(Department deptObj)
        {
            try
            {
                List<Department> deptlist = new List<Department>();
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                string sQuery = "insert into department values('" + deptObj.vDept_name + "'," + "'" + deptObj.vProject_name + "')";
                SqlCommand cmd = new SqlCommand(sQuery, conn);
                cmd.ExecuteNonQuery();
                deptlist.Add(deptObj);
                return deptlist;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { conn.Close(); }
        }
        #endregion

        /// <summary>
        /// This will use to get the login detail of the Employees
        /// </summary>
        /// 
        /// <returns></returns>
        #region Get Login and logout detail
        Log logobj = new Log();
        [HttpGet]
        [ActionName("GetLog")]
        public List<Log> getLog()
        {
            
            try
            {
                List<Log> loglist = new List<Log>();
                string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
                conn = new SqlConnection(conStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from check_log", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for(int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                         logobj.vemail= (Convert.ToString(dt.Rows[i]["vemail"]));
                        logobj.dtlogin = (Convert.ToDateTime(dt.Rows[i]["dtlogin"]));
                        logobj.dtlogout = (Convert.ToDateTime(dt.Rows[i]["dtlogout"]));
                        loglist.Add(logobj);
                    }
                }
                conn.Close();
                return loglist;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion


    }
}