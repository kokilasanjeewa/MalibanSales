using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateSaleQuantityDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public decimal quantity { get; set; }
    [Required]
    public int branchLocationId { get; set; }
    [Required]
    public decimal dSalePrice { get; set; }
    public string number { get; set; }
    [Required]
    public decimal dCostPrice { get; set; }
    public Int64 transactionDetailId { get; set; }
    public int paymentId { get; set; }
    }
}