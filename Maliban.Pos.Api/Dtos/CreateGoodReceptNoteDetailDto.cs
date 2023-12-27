using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateGoodReceptNoteDetailDto
  {
    [Required]
    [Display(Name = "Quantity")]
    public int? ItemQuantity { get; set; }
    [Required]
    [Display(Name = "Quantity")]
    public int? BalanceItemQuantity { get; set; }
    [Display(Name = "Free Quantity")]
    public int? FreeItemQuantity { get; set; }
    //[Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Free Item Cost Price")]
    public decimal? FreeItemCost { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Purchase Item Date")]
    public DateTime InvoiceItemDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; } = DateTime.Now;
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Cost Price")]
    public decimal? ItemCostPrice { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Sale Price")]
    public decimal? ItemSalePrice { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Discount")]
    public decimal? ItemDiscount { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Item Discount Amount")]
    public decimal? ItemDiscountAmount { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ProductExpireDate { get; set; }
    [Required]
    public string BatchNumber { get; set; }
    [Required]
    public string GoodReceptNoteLine { get; set; }
    [Required]
    public int GoodReceptNoteId { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    [Required]
    public Int64 PriceId { get; set; }


  }
}