using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CustomerMasterSettingListDto
    {
        [Required]
        public int CustomerMasterSettingId { get; set; }
        [Required]
        public string CustomerMasterSettingUid { get; set; } = Guid.NewGuid().ToString();
        [StringLength(100, ErrorMessage = "The CustomerMasterSetting Description cannot exceed 100 characters. ")]
        public string CustomerMasterSettingDescription { set; get; }

        public virtual BranchLocation BranchLocation { set; get; }
        [Required]
        public int? BranchLocationId { get; set; }
        public string CustomerCategory { get; set; }
        public CustomerCategoryCode CustomerCategoryCode { get; set; }
        public string BillingCycleStartDate { get; set; }
        public string BillingCycleEndDate { get; set; }
        public int CreditHoldingDays { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreaditBalancePerDays { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }
    }
}
