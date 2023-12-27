using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  public class Stock 
  {
    [Key]
    [Display(Name = "Stock Id")]
    public Int64 StockId { get; set; }
    [Required]
    public string StockUid { get; set; } = Guid.NewGuid().ToString();
    [Required] public Int64? ProductId { get; set; }
    [Required] public Int64? PriceId { get; set; }

    [Required]
    [Display(Name = "Unit InStock ")]
    public int UnitsInStock { get; set; }
    [Display(Name = "Recorder Level")]
    public ReorderLevel ReorderLevel { get; set; }
    public int? AlertQuantity { get; set; }
    [Required]
    [Display(Name = "Sale Quantity")]
    public int BalanceQuantity { get; set; }
    public bool ProductAvailable { get; set; } = true;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime TransferDate { get; set; }
    public int? TransferBranchLocationId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ProductExpireDate { get; set; }
    public bool Active  
    {
      get
      {
        if (this.ProductExpireDate<DateTime.Now.Date)
        {
      return true;
    }
        return false;
    }
  }
  [DataType(DataType.Date)]
  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTime ModifiedDate { get; set; }
  [DataType(DataType.Date)]
  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTime CreateDate { get; set; }
  [Required]
  [Display(Name = "Create By ")]
  [StringLength(100, ErrorMessage = "The CreatedBY cannot exceed 100 characters. ")]
  public string CreatedBy { get; set; }
  [Display(Name = "Modified By")]
  [StringLength(100, ErrorMessage = "The ModifiedBy cannot exceed 100 characters. ")]
  public string ModifiedBy { get; set; }
  public virtual Product Product { get; set; }
  [ForeignKey("StoreId")]
  public virtual Store Store { get; set; }
  [ForeignKey("BranchLocationId")]
  public virtual BranchLocation BranchLocation { get; set; }
  [ForeignKey("PriceId")]
  public virtual Price Price { get; set; }
  [Required] public int? BranchLocationId { get; set; }
  [Required] public int? StoreId { get; set; }
}

}