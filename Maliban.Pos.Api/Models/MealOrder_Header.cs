using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
    public class MealOrder_Header
    {
        [Key]
        public long mohid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RequestDate { get; set; }
        [Required]
        public int MealTypeId { get; set; }
        [Required]
        public int MI_Employer_EmployeeId { get; set; }
        [Required]
        public int BranchLocationId { get; set; }
        public int CreatedUserId { get; set; }
        public int CreatedSystem { get; set; }
        public bool bActive { get; set; }
        public long? SaleDetailId { get; set; }
    }
}
