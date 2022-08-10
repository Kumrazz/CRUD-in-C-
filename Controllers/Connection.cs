using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManufacturerAPI.Controllers
{
    public class Connection
    {

        public SqlDataReader ExecuteReader(string sQuery)
        {
            string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();
            
            SqlCommand cmd=new SqlCommand(sQuery,connection);
            SqlDataReader reader = cmd.ExecuteReader();

            return reader;
        }

        public void ExecuteScalar(string sQuery)
        {
            string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();
            
            SqlCommand cmd = new SqlCommand(sQuery, connection);
            Object iExecScalar = cmd.ExecuteScalar();
            
        }
    }
}