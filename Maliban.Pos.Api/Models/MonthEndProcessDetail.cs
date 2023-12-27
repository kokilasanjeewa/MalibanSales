using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{

    public class MonthEndProcessDetail
    {
        [Key]
        [Display(Name = "Month End Process Detail Id")]
        public Int64 MEPDetailId { get; set; }
        public int MI_Employer_EmployeeId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditLimit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditBalance { get; set; } = 0;
        [Required]
        public Int64 MEPHeaderId { get; set; }
        [ForeignKey("MEPHeaderId")]
        public virtual MonthEndProcessHeader MonthEndProcessHeader { get; set; }
    }
}