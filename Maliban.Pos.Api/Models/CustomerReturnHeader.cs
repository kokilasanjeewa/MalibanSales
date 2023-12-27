using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
    public class CustomerReturnHeader : BaseModel
    {
        [Key]
        public Int64 CustomerReturnHeaderId { get; set; }
        [Required]
        public string CustomerReturnHeaderUid { get; set; } = Guid.NewGuid().ToString();
        [StringLength(100, ErrorMessage = "The CustomerReturnNote cannot exceed 100 characters. ")]
        public string CustomerReturnNote { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CustomerReturnDate { get; set; }
        [Required]
        public int BranchLocationId { get; set; }

        [Required]
        public bool IsProcess { get; set; } = false;

        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public decimal CustomerReturnAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public decimal PreviousBillAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PaidAmount { get; set; }
        public virtual BranchLocation BranchLocation { get; set; }
        [Required]
        public int MI_Employer_EmployeeId { get; set; }
        [Required(ErrorMessage = "Please enter Customer.")]
        public string EmployeeCode { get; set; }
        public virtual MI_Employer_Employee MI_Employer_Employee { get; set; }
        public virtual ICollection<CustomerReturnDetail> Lines { get; set; }
        [Required]
        [Display(Name = "ApplicationUser Id")]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        [Required]
        public Int64 SaleHeaderId { get; set; }
        [ForeignKey("SaleHeaderId")]
        public virtual SaleHeader SaleHeader { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditBalance { get; set; } = 0;
    }
}