using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeData.Models
{

    public class Employee 
    {
        [Key]
        public string empid { get; set; }
        public string empname { get; set; }
        public string empdesignation { get; set; }
        public string empcontact { get; set; }
    }
}
