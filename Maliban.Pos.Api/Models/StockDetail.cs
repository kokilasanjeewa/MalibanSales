using System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  public class StockDetail : BaseModel
  {
    [Key]
    public Int64 StockDetailId { get; set; }
    public string StockDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int StockQuantity { get; set; }
    [Display(Name = "Recorder Level")]
    public ReorderLevel ReorderLevel { get; set; }
    public int? AlertQuantity { get; set; }
    [Required]
    [Display(Name = "Sale Quantity")]
    public int BalanceQuantity { get; set; }
    public bool ProductAvailable { get; set; } = true;
    [Display(Name = "Units OnOrder")]
    public int? UnitsOnOrder { get; set; }

    [Required]
    public int StoreAreaId { get; set; }
    public ICollection<StoreArea> StoreArea { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public Int64? StoreId { get; set; }
    public virtual Store Store { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    public virtual Product Product { get; set; }
    [Required]
    public int? BinId { get; set; }
    public virtual Bin Bin { get; set; }
    [Required]
    public Int64 GoodReceptNoteDetailId { get; set; }
    public virtual GoodReceptNoteDetail GoodReceptNoteDetail { get; set; }
    [Required]
    public Int64 PriceId { get; set; }
    public virtual Price Price { get; set; }
  }
}