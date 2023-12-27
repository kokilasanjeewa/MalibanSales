using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateGrnQuantityDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public int quantity { get; set; }
    [Required]
    public Int64 purchaseOrderHeaderId { get; set; }
    [Required]
    public Int64 SupplierPriceId { get; set; }
    }
}