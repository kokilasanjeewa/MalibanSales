using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class StockIssueHeader : BaseModel
  {
    [Key]
    public Int64 StockIssueHeaderId { get; set; }
    [Required]
    public string StockIssueHeaderUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int? StoreAreaId { get; set; }
    public StoreArea StoreArea { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public Int64 StoreId { get; set; }
    public virtual Store Store { get; set; }

    public virtual ICollection<StockIssueDetail> StockIssueDetail { get; set; }
  }
}