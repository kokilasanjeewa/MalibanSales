using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateProductGrnItemDetailDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public decimal quantity { get; set; }
    public decimal dNetQuantity { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string createdBy { get; set; }
    public int batchnumber { get; set; }
    [Required]
    public int priceId { get; set; }
    [Required]
    public decimal freequantity { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime productExpiryDate { get; set; }=DateTime.Now;
    [Required]
    public Int64 PurchaseOrderHeaderId { get; set; }
    [Required]
    public decimal RetailPrice { get; set; }

  }
}