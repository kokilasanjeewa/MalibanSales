using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CreatePurchaseOrderApproveStatusyDto
    {
        [Required]
        public string barCode { get; set; }
        [Required]
        public int approveStatus { get; set; }
        [Required]
        public string createdBy { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int BranchLocationId { get; set; }
        //  [Required]
        public int SupplierPriceId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductExpiryDate { get; set; }
    }
}
