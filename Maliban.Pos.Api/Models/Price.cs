using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  public class Price : BaseModel
  {
    [Key]
    public Int64 PriceId { get; set; }
    [Required]
    public string PriceUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Cost Price")]
    [ConcurrencyCheck]
    public decimal UnitPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Modified Cost Price")]
    [ConcurrencyCheck]
    public decimal ModifiedUnitPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "MaximumRetail Price")]
    public decimal MaximumRetailPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Sale Price")]
    public decimal SalePrice { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Discount Price")]
    public decimal DiscountPrice { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Discount Precentage")]
    public decimal DiscountPrecentage { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Promotion Price")]
    public decimal PromotionPrice { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Promotion Precentage")]
    public decimal PromotionPrecentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Tax Precentage")]
    public decimal? TaxPrecentage { get; set; }
    public TaXMethod TaXMethod { get; set; }
    [Required]
    public StatusType StatusType { get; set; }
    [JsonIgnore]
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [JsonIgnore]
    [ForeignKey("BranchLocationId")]
    public virtual BranchLocation BranchLocation { get; set; }
    public virtual ICollection<CustomerOrderDetail> CustomerOrderDetails { get; set; }
    public virtual ICollection<CustomerReturnDetail> CustomerReturnDetails { get; set; }
    public virtual ICollection<StockDetail> StockDetails { get; set; }
    public virtual ICollection<Stock> Stocks { get; set; }
    public virtual ICollection<ShopStock> ShopStocks { get; set; }
    [JsonIgnore]
    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }
    [JsonIgnore]
    public virtual ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
    [JsonIgnore]
    public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }
 
    }
}
