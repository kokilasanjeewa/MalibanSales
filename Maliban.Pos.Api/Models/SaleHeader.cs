using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Maliban.Pos.Api.Models
{
    public class SaleHeader : BaseModel
    {
        [Key]
        public Int64 SaleHeaderId { get; set; }
        [Required]
        public string SaleHeaderUid { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime OrderTimeStamp {  get{return DateTime.Now; } set{} }
        public DateTime OrderTimeStamp { get; set; }
        [Required]
        public bool TransactStatus { get; set; }
        [StringLength(200, ErrorMessage = "The ErrorLoc cannot exceed 200 characters. ")]
        public string ErrorLoc { get; set; }
        [StringLength(100, ErrorMessage = "The ErrorMessage cannot exceed 100 characters. ")]
        public string ErrorMessage { get; set; }
        [Required]
        public bool Paid { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime PaymentDate {  get{return DateTime.Now; } set{} }
        public DateTime PaymentDate { get; set; }
        [Required]
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        [Required]
        [Display(Name = "BranchLocation Id")]
        public int BranchLocationId { get; set; }
        [ForeignKey("BranchLocationId")]
        public virtual BranchLocation BranchLocation { get; set; }
        [Required]
        [Display(Name = "ApplicationUser Id")]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<SaleDetail> Lines { get; set; }
        [Required]
        public int MI_Employer_EmployeeId { get; set; }
        [Required]
        [StringLength(5)]
        public string EmployeeCode { get; set; }
        public virtual MI_Employer_Employee MI_Employer_Employee { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public decimal CustomerReturnAmount { get; set; } = 0;
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PreviousAmount { get; set; } = 0;
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditBalance { get; set; } = 0;
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditLimit { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public decimal CustomerCashReturnAmount { get; set; } = 0;
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PreviousCashAmount { get; set; } = 0;
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CashBalance { get; set; } = 0;
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CashLimit { get; set; } = 0;
        [StringLength(12)]
        public string PaymentReferenceId { get; set; }
        public int CardMethodId { get; set; } = 0;

    }

}
