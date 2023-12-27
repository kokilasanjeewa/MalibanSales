using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
  public class SupplierPriceListDto
  {
    public Int64 SupplierPriceId { get; set; }
    [Required]
    public string SupplierPriceUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [ConcurrencyCheck]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Cost Price")]
    public decimal UnitPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Sale Price")]
    public decimal MaximumRetailPrice { get; set; }
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
    [ConcurrencyCheck]
    [Timestamp]
    public byte[] RowVersion { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Tax Precentage")]
    public decimal? TaxPrecentage { get; set; }
    public TaXMethod TaXMethod { get; set; }
    [Required]
    public StatusType StatusType { get; set; }
    public virtual Product Product { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    [Required]
    public int SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime PriceDate { get; set; }
    public bool Active { get; set; }
    public bool IsDeleted { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Profit")]
        public decimal Profit { get; set; } = 0;
    }
}