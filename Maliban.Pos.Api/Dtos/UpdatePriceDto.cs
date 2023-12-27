using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdatePriceDto
  {
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
    public decimal? DiscountPrice { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Discount Precentage")]
    public decimal? DiscountPrecentage { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Promotion Price")]
    public decimal? PromotionPrice { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Promotion Precentage")]
    public decimal? PromotionPrecentage { get; set; } = 0;
    [Required]
    [Display(Name = "Vender Name")]
    public string VenderName { get; set; }
    [ConcurrencyCheck]
    [Timestamp]
    public byte[] RowVersion { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Tax Precentage")]
    public decimal? TaxPrecentage { get; set; } = 0;
    public TaXMethod TaXMethod { get; set; } = 0;


  }
}