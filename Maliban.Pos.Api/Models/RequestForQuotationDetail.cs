using System.Numerics;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class RequestForQuotationDetail : BaseModel
  {
    [Key]
    public Int64 RequestForQuotationDetailId { get; set; }
    [Required]
    public string RequestForQuotationDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int Quantity { get; set; }
    [Required]
    [Display(Name = "Product Name")]
    [StringLength(50, ErrorMessage = "The Modified cannot exceed 50 characters. ")]
    public string ProductName { get; set; }
    [Required]
    [Display(Name = "Product Description")]
    [StringLength(100, ErrorMessage = "The Product Description cannot exceed 100 characters. ")]
    public string ProductDescription { get; set; }
    [StringLength(5, ErrorMessage = "The Size cannot exceed 5 characters. ")]
    public string Size { get; set; }
    [StringLength(30, ErrorMessage = "The Color cannot exceed 30 characters. ")]
    public string Color { get; set; }
    [Display(Name = "Unit Weight")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitWeight { get; set; }
    [StringLength(200, ErrorMessage = "The Note cannot exceed 200 characters. ")]
    public string Note { get; set; }
    public int? BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
    public Int64 ProductId { get; set; }
    public virtual Product Product { get; set; }
    [Required]
    public Int64 RequestForQuotationHeaderId { get; set; }
    public virtual RequestForQuotationHeader RequestForQuotationHeader { get; set; }


  }

}