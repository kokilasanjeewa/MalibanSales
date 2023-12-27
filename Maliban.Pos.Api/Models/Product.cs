using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  [Table("Products")]
  public class Product : BaseModel
  {
    [Key]
    public Int64 ProductId { get; set; }
    [Required]
    public string ProductUid { get; set; } = Guid.NewGuid().ToString();
    //[Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ExpiryDate { get; set; }
    [ConcurrencyCheck]
    [Required]
    [Display(Name = "Barcode")]
    [StringLength(100, ErrorMessage = "The Barcode cannot exceed 100 characters. ")]
    public string Barcode { get; set; } = "New" + Guid.NewGuid().ToString();
    [Required]
    [Display(Name = "Product Name")]
    [StringLength(50, ErrorMessage = "The ProductName cannot exceed 100 characters. ")]
    public string ProductName { get; set; }
    [Display(Name = "Product Description")]
    [StringLength(200, ErrorMessage = "The ProductDescription cannot exceed 200 characters. ")]

    public string ProductDescription { get; set; }
    [StringLength(5, ErrorMessage = "The Size cannot exceed 5 characters. ")]
    public string Size { get; set; }
    [StringLength(30, ErrorMessage = "The Color cannot exceed 30 characters. ")]
    public string Color { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Width { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Height { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Length { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    //[Display(Name = "Unit Weight")]
    public decimal UnitWeight { get; set; } = 0;
    [StringLength(100, ErrorMessage = "The Note cannot exceed 100 characters. ")]
    public string Note { get; set; }
    public virtual ICollection<ProductCategory> ProductCategory { get; set; }
    public virtual ICollection<ProductSubCategory> ProductSubCategory { get; set; }
    public virtual ICollection<Photo> Photos { get; set; }
    public virtual ICollection<Price> Prices { get; set; }

    // public virtual ICollection<Price> Price { get; set; }
    [ForeignKey("VendorId")]
    public virtual Vendor Vendor { get; set; }
    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }
    [ForeignKey("SubCategoryId")]
    public virtual SubCategory SubCategory { get; set; }
    [ForeignKey("UnitOfMeasurementId")]
    public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
    [ForeignKey("BrandId")]
    public virtual Brand Brand { get; set; }
    [ConcurrencyCheck]
    [Timestamp]
    public byte[] RowVersion { get; set; }
    [Required]
    public StatusType StatusType { get; set; }
    public ICollection<SupplierProduct> SupplierProducts { get; set; }
    public ICollection<SupplierPrice> SupplierPrices { get; set; }
    public int? UnitOfMeasurementId { get; set; }
    public int? BrandId { get; set; }
    public int? SupplierId { get; set; }
    //  [Required]
    public int? CategoryId { get; set; }
    //  [Required]
    public int? SubCategoryId { get; set; }
    public int? VendorId { get; set; }

  }

}


