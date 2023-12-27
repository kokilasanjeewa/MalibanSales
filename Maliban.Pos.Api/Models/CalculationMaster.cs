using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
    public class CalculationMaster : BaseModel
    {
        [Key]
        public int CalculationMasterId { get; set; }
        [Required]
        public string CalculationMasterUid { get; set; } = Guid.NewGuid().ToString();
        [StringLength(100, ErrorMessage = "The BrandDescription cannot exceed 100 characters. ")]
        public string CalculationMasterDescription { set; get; }

        public virtual BranchLocation BranchLocation { set; get; }
        [Required]
        public int? BranchLocationId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal SalePricePrecentageCompany { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal SalePricePrecentageEmployee { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ManagerCreditLimitPrecentage { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal StaffCreditLimitPrecentage { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal WorkerCreditLimitPrecentage { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal VATPrecentage { get; set; } = 0;
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal NBTPrecentage { get; set; } = 0;
    }
}