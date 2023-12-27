using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateProductDto : BaseDto
  {
   // [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ExpireDate { get; set; }
    [Required]
    [Display(Name = "Stock Keeping Unit")]
    public string SKU { get; set; }
    [Required]
    [Display(Name = "Barcode")]
    public string Barcode { get; set; }
    // [Required]
    //  [Display(Name = "Vender Name")]
    public int? VendorId { get; set; }
    [Required]
    [Display(Name = "Product Name")]
    public string ProductName { get; set; }
    [Required]
    [Display(Name = "Product Description")]
    public string ProductDescription { get; set; }
    // [Required]
    public int? SupplierId { get; set; }
    //  [Required]
    public int? CategoryId { get; set; }
    //  [Required]
    public int? SubCategoryId { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    // [Required]
    [Display(Name = "Unit Weight")]
    public decimal UnitWeight { get; set; } = 0;
    public string Note { get; set; }
    // [Required]
    public int? UnitOfMeasurementId { get; set; }
    public int? BrandId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string RowVersion { get; set; }

  }
}