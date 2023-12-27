using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class GetStockAdjustmentItemQuantityByProductDto
    {
        [Required]
        public string barCode { get; set; }
        [Required]
        public int branchLocationId { get; set; }
    }
}
