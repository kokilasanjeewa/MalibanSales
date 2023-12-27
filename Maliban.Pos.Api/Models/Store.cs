using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class Store : BaseModel
  {
    [Key]
    public Int64 StoreId { get; set; }
    public string StoreUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50, ErrorMessage = "The Store Name cannot exceed 50 characters. ")]
    public string StoreName { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The Store Description cannot exceed 100 characters. ")]
    public string StoreDescription { get; set; }
    public ICollection<StoreArea> StoreArea { get; set; }
    [Required]
    public int? BranchLocationId { get; set; }
    [ForeignKey("BranchLocationId")]
    public virtual BranchLocation BranchLocation { get; set; }
    public virtual ICollection<StockMovementHeader> StockMovementHeaders { get; set; }
    public virtual ICollection<StockDetail> StockDetails { get; set; }
    public virtual ICollection<StockIssueHeader> StockIssueHeaders { get; set; }
    public virtual ICollection<Bin> Bins { get; set; }
/*     public virtual ICollection<ShopStock> ShopStocks { get; set; }
 */    public virtual ICollection<Stock> Stocks { get; set; }



  }
}