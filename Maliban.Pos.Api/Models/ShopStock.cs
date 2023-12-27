using System.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  public class ShopStock
  {
    [Key]
    public Int64 ShopStockId { get; set; }
    [Required]
    public string ShopStockUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [Display(Name = "Unit InStock ")]
    public decimal UnitsInStock { get; set; }
    [Display(Name = "Recorder Level")]
    public ReorderLevel ReorderLevel { get; set; }
    public int? AlertQuantity { get; set; } = 0;
    [Display(Name = "Sale Quantity")]
    public decimal SaleQuantity { get; set; } = 0;
    public bool ProductAvailable { get; set; } = true;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime TransferDate { get; set; }
    public int? TransferBranchLocationId { get; set; } = 0;
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ProductExpireDate { get; set; }
    private bool active;
    public bool Active
    {
      get
      {
        if (DateTime.Now.Date < this.ProductExpireDate)
        {
          return active = true;
        }
        return active = false;
      }
      set
      {
        if (value == true)
          active = value;
      }
    }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifyDate { get; set; }
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
    [Required]
    public Int64 ProductId { get; set; }
    public virtual Product Product { get; set; }
    /*     [Required]
        public Int64? StoreId { get; set; }
        public virtual Store Store { get; set; } */
    [Required]
    public int BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public Int64 PriceId { get; set; }
    public virtual Price Price { get; set; }
  }
}
