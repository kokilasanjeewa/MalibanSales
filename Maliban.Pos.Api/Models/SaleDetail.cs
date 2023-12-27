using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class SaleDetail : BaseModel
  {
    [Key]
    [Display(Name = "Order Detail Id")]
    public Int64 SaleDetailId { get; set; }
    [Required]
    public string SaleDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [Display(Name = "Product Id")]
    public Int64 ProductId { get; set; }
    [Required]
    [Display(Name = "Price Id")]
    public Int64 PriceId { get; set; }
    [NotMapped]
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }
    [NotMapped]
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ItemPrice { get; set; }
    [NotMapped]
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ItemDiscount { get; set; } = 0;
    [NotMapped]
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Total { get; set; }
    [NotMapped]
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? SubTotal { get; set; }
    [Required]
    public decimal Quantity { get; set; }
    [NotMapped]
    [Required]
    public decimal StockQuantity { get; set; }
    [Required]
    public decimal ReturnQuantity { get; set; } = 0;
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Bill Date")]
    public DateTime BillDate { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }

    [Required]
    public bool IsReturn { get; set; } = false;
    [Required]
    public bool Paid { get; set; } = false;
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
    [ForeignKey("PriceId")]
    public virtual Price Price { get; set; }
    [Required]
    public Int64 SaleHeaderId { get; set; }
    [ForeignKey("SaleHeaderId")]
    public virtual SaleHeader SaleHeader { get; set; }
    [NotMapped]
    public bool ActiveBill { get; set; } = false;
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public override string CreatedBy { get; set; }
    [StringLength(100, ErrorMessage = "The ModifiedBy cannot exceed 100 characters. ")]
    public override string ModifiedBy { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ProductExpireDate { get; set; }
    [Required]
    public Int64 TransactionDetailId { get; set; }
    [ForeignKey("TransactionDetailId")]
    public virtual TransactionDetail TransactionDetail { get; set; }


    }
}