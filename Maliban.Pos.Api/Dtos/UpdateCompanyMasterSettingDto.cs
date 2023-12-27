using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class UpdateCompanyMasterSettingDto
    {
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
