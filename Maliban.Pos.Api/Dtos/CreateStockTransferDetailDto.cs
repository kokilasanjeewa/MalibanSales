using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateStockTransferDetailDto
    {
        [Required]
        public decimal quantity { get; set; }
        [Required]
        public int branchLocationId { get; set; }
        [Required]
        public string barCode { get; set; }
        public Int64 priceId { get; set; }
        public int paymentId { get; set; } = 1;
        public string StockTransferDetailUid { get; set; } = System.Guid.NewGuid().ToString();

    }

}