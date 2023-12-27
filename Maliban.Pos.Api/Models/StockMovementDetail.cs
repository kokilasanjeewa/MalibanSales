using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class StockMovementDetail : BaseModel
  {
    [Key]
    public Int64 StockMovementDetailId { get; set; }
    [Required]
    public string StockMovementDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public Int64 ProductId { get; set; }
    public virtual Product Product { get; set; }
    [Required]
    public int MovementQuantity { get; set; }
    [Required]
    public int PreviousQuantity { get; set; }
    [Required]
    public int CurrentQuantity { get; set; }
    [Required]
    public Int64 StockMovementHeaderId { get; set; }
    public virtual StockMovementHeader StockMovementHeader { get; set; }

  }
}