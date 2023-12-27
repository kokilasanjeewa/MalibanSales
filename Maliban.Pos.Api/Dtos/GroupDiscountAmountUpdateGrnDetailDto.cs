using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class GroupDiscountAmountUpdateGrnDetailDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public decimal groupDiscountAmountAmount { get; set; }
    [Required]
    public Int64 purchaseOrderHeaderId { get; set; }
    [Required]
    public Int64 SupplierPriceId { get; set; }
    }
}