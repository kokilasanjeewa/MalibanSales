using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Maliban.Pos.Api.Models
{
  public class CustomerOrderDetail : BaseModel
  {
    [Key]
    public Int64 CustomerOrderDetailId { get; set; }
    [Required]
    public string CustomerOrderDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public Int64 CustomerOrderHeaderId { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    [Required]
    public Int64 PriceId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Amount")]
    public decimal Amount { get; set; }
    public int? CurrentQuantity { get; set; }
    public int? ReturnQuantity { get; set; }
    public virtual CustomerOrderHeader CustomerOrderHeader { get; set; }
    public virtual Product Product { get; set; }
    public virtual Price Price { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }

  }
}