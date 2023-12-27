using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  public class StockMovementHeader : BaseModel
  {
    [Key]
    public Int64 StockMovementHeaderId { get; set; }
    [Required]
    public string StockMovementHeaderUid { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public int BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public Int64? StoreId { get; set; }
    [JsonIgnore]
    [ForeignKey("StoreId")]
    public virtual Store Store { get; set; }
  }
}