using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CompanyMasterSettingListDto
    {
        [Required]
        public int CalculationMasterId { get; set; }
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
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }
    }
}
