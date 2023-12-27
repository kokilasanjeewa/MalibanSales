using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateCustomerMasterSettingDto
    {
        [StringLength(100, ErrorMessage = "The CustomerMasterSetting Description cannot exceed 100 characters. ")]
        public string CustomerMasterSettingDescription { set; get; }
        [Required]
        public int? BranchLocationId { get; set; }
        public CustomerCategoryCode CustomerCategoryCode { get; set; }
        public int BillingCycleStartDate { get; set; }
        public int BillingCycleEndDate { get; set; }
        public int CreditHoldingDays { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreaditBalancePerDays { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
        public string CreatedBy { get; set; }
    }
}
