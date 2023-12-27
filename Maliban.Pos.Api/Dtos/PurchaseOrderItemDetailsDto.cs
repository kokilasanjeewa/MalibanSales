using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class PurchaseOrderItemDetailsDto
  {
    public Int64 PurchaseOrderDetailId { get; set; }
    [Required]
    public string PurchaseOrderDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public Int64 PurchaseOrderHeaderId { get; set; }
    [Required]
    public Int64? ProductId { get; set; }
    [Required]
    public Int64 SupplierPriceId { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifyDate { get; set; }
    [Required]
    public int Quantity { get; set; }
    /*  [Required]
      [Column(TypeName = "decimal(18, 2)")]
      [DisplayFormat(DataFormatString = "{0:C}")]
      [Display(Name = "Order Price")]
      public decimal? OrderPrice { get; set; } */
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Amount")]
    public decimal Amount { get; set; }
    [Required]
    public int? CurrentQuantity { get; set; } = 0;
    public int? ReturnQuantity { get; set; } = 0;
    public int? BonusQuantity { get; set; } = 0;
    public int? GRNQuantity { get; set; } = 0;
    public int? FreeQuantity { get; set; } = 0;
    public virtual PurchaseOrderHeader PurchaseOrderHeaders { get; set; }
    public virtual Product Product { get; set; }
    public virtual SupplierPrice SupplierPrice { get; set; }
  }
}