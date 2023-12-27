using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class RemoveSaleDetailDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public int branchLocationId { get; set; }
    [Required]
    public decimal dSalePrice { get; set; }
    [Required]
    public Int64 transactionDetailId { get; set; }

    }
}