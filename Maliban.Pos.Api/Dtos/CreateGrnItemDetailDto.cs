using System;
using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateGrnItemDetailDto
  {
    [Required]
    public Int64 productId { get; set; }
    [Required]
    public int quantity { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string createdBy { get; set; }
    public int batchnumber { get; set; }
    [Required]
    public int priceId { get; set; }
    [Required]
    public int freequantity { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime productExpiryDate { get; set; }
    [Required]
    public Int64 PurchaseOrderHeaderId { get; set; }
    [Required]
    public decimal RetailPrice { get; set; }


  }
}