using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Models
{
    public class mas_Employee
    {   
        [Key]
        public Int64 AutoId { get; set; }
        [StringLength(100, ErrorMessage = "The Name cannot exceed 100 characters. ")]
        public string Name { get; set; }
        [StringLength(10, ErrorMessage = "The Location cannot exceed 10 characters. ")]
        public string Location { get; set; }
        public int link_Department { get; set; }
        public int link_Desgnation { get; set; }
        [StringLength(200, ErrorMessage = "The email cannot exceed 200 characters. ")]

        public string email { get; set; }
        [StringLength(200, ErrorMessage = "The EmployeeNo cannot exceed 200 characters. ")]

        public string EmployeeNo { get; set; }
        public DateTime Date_Of_Join { get; set; }
        public DateTime Left_Date { get; set; }
        [StringLength(50, ErrorMessage = "The Grading cannot exceed 50 characters. ")]

        public string Grading { get; set; }
        public bool bActive { get; set; }
        [StringLength(50, ErrorMessage = "The Category cannot exceed 50 characters. ")]

        public string Category { get; set; }
        [StringLength(10, ErrorMessage = "The Barcodenumber cannot exceed 10 characters. ")]

        public string Barcodenumber { get; set; }
        [StringLength(20, ErrorMessage = "The StaffCat cannot exceed 200 characters. ")]

        public string StaffCat { get; set; }

    }
}
