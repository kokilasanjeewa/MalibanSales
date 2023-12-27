using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class RemovePurchaseOrderDetailDto
    {
        [Required]
        public string barCode { get; set; }
        [Required]
        public int branchLocationId { get; set; }
        [Required]
        public int SupplierPriceId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductExpiryDate { get; set; }
    }
}
