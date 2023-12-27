using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePriceDto
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
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Tax Precentage")]
    public decimal? TaxPrecentage { get; set; } = 0;
    public TaXMethod TaXMethod { get; set; } = 0;
    [Required]
    public Int64? ProductId { get; set; }
    [Required]
    public int? BranchLocationId { get; set; }
  }
}