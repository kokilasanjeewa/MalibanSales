using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class StoreArea : BaseModel
  {
    [Key]
    public int StoreAreaId { get; set; }
    [Required]
    public string StoreAreaUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50, ErrorMessage = "The Store Area Name cannot exceed 100 characters. ")]
    public string StoreAreaName { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The Store Area Description cannot exceed 100 characters. ")]
    public string StoreAreaDescription { get; set; }

    public virtual Store Store { get; set; }
    [Required]
    public int StoreId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    public virtual ICollection<Rack> Racks { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    public virtual ICollection<StockIssueHeader> StockIssueHeaders { get; set; }
    public virtual ICollection<Bin> Bins { get; set; }

  }
}