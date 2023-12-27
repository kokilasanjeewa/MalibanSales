using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateStockAdjustmentApproveStatusDto
    {
        [Required]
        public string barCode { get; set; }
        [Required]
        public int approveStatus { get; set; }
        [Required]
        public int branchLocationId { get; set; }
        [Required]
        public Int64 PriceId { get; set; }
        [Required]
        public Int64 TransactionDetailId { get; set; }
    }
}