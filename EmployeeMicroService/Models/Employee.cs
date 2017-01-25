using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeMicroService.Models
{
    public class Employee
    {
        public Int32 ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsManager { get;set;}
        public Int32 Grade { get; set; }
    }
}