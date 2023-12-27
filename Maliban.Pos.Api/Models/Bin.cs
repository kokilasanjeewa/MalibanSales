using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class Bin : BaseModel
  {
    [Key]
    public Int64 BinId { get; set; }
    [Required]
    public int? ShelfNumber { set; get; }
    [Required]
    public int? BinNumber { get; set; }
    [Required]
    public string BinUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(100, ErrorMessage = "The BinDescription cannot exceed 100 characters. ")]
    public string BinDescription { set; get; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? BinWidth { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? BintHeight { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? BinLength { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? MaxTolerableWeight { get; set; }
    public virtual Rack Rack { set; get; }
    public virtual StoreArea StoreArea { set; get; }
    public virtual Store Store { set; get; }
    public virtual BranchLocation BranchLocation { set; get; }
    [Required]
    public int? BranchLocationId { get; set; }
    [Required]
    public Int64? StoreId { get; set; }
    [Required]
    public int? StoreAreaId { get; set; }
    [Required]
    public int? RackId { get; set; }
    [Required]
    public string BinCode
    {
      get
      {
        return this.BranchLocationId + "-" + this.StoreId + "-" + this.StoreAreaId + "-" + this.RackId + "-" + this.ShelfNumber + "-" + this.BinNumber;

      }
    }
    public virtual ICollection<StockDetail> StockDetails { get; set; }
  }
}