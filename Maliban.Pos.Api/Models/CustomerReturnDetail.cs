using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class CustomerReturnDetail : BaseModel
  {
    [Key]
    public Int64 CustomerReturnDetailId { get; set; }
    [Required]
    public string CustomerReturnDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public Int64 CustomerReturnHeaderId { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    [Required]
    public Int64 PriceId { get; set; }
    [Required]
    public decimal ReturnQuantity { get; set; }
    [Required]
    public decimal SaleQuantity { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Amount")]
    public decimal Amount { get; set; }
    public decimal? CurrentQuantity { get; set; }
    public decimal? BalanceQuantity { get; set; }
    public virtual CustomerReturnHeader CustomerReturnHeader { get; set; }
    public virtual Product Product { get; set; }
    public virtual Price Price { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ProductExpireDate { get; set; }
    [Required]
    public Int64 SaleDetailId { get; set; }
    [ForeignKey("SaleDetailId")]
    public virtual SaleDetail SaleDetail { get; set; }
    [Required]
    public Int64 TransactionDetailId { get; set; }
    [ForeignKey("TransactionDetailId")]
    public virtual TransactionDetail TransactionDetail { get; set; }
    }
}