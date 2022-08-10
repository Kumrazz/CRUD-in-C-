using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManufacturerAPI.Models
{
    public class Employee
    {
        public string vFirst_name { get; set; }
        public string vMiddle_name { get; set; }
        public string vLast_name { get; set; }
        public string vemail { get; set; }
        public long nmob_number { get; set; }
        public string vPosition { get; set; }
        public char cGender { get; set; }
	    public DateTime dDOB {get;set;}
        public DateTime dDOJ { get; set; }
	    public string vDepartment { get; set; }
	    public string vAddress { get; set; }
	    public int iManId { get; set; }
	    public int mSalary { get; set; }
    }
}