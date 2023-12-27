using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateProductDto
  {
   // [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ExpireDate { get; set; }
    [Required]
    public string CreatedBy { get; set; }
    [Display(Name = "Stock Keeping Unit")]
    public string SKU { get; set; }
    [Display(Name = "Barcode")]
    public string Barcode { get; set; } = "New" + Guid.NewGuid().ToString();
    [Required]
    [Display(Name = "Product Name")]
    public string ProductName { get; set; }
    [Display(Name = "Product Description")]
    public string ProductDescription { get; set; }
    public int? CategoryId { get; set; }
    public int? SubCategoryId { get; set; }
    public int? VendorId { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    [Display(Name = "Unit Weight")]
    public string UnitWeight { get; set; }
    public string Note { get; set; }
    [Required]
    public int? UnitOfMeasurementId { get; set; }
    public int? BrandId { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Width { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Height { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Length { get; set; }

  }
}