using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePurchaseReturnDetailsDto
  {
    [Required]
    public Int64 priceId { get; set; }
    [Required]
    public decimal quantity { get; set; }
    [Required]
    public string createdBy { get; set; }
    [Required]
    public Int64 productId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    public Int64 goodReceptNoteHeaderId { get; set; }
    }
}