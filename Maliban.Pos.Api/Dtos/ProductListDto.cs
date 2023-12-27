using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class ProductListDto
  {
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifiedDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ExpireDate { get; set; }
    [Required]
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    [Required]
    [Display(Name = "Stock Keeping Unit")]
    public string SKU { get; set; }
    [ConcurrencyCheck]
    [Required]
    [Display(Name = "Barcode")]
    public string Barcode { get; set; }
    [Required]
    [Display(Name = "Vender Name")]
    public string VenderName { get; set; }
    [Required]
    [Display(Name = "Product Name")]
    public string ProductName { get; set; }
    [Required]
    [Display(Name = "Product Description")]
    public string ProductDescription { get; set; }

    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int SubCategoryId { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    [Required]
    [Display(Name = "Unit Weight")]
    public string UnitWeight { get; set; }
    public string Note { get; set; }
    [Required]
    public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
    public string BrandId { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey("CategoryId")]
    public virtual ICollection<ProductCategory> ProductCategory { get; set; }
    public virtual ICollection<GoodReceptNoteDetail> GoodReceptNoteDetail { get; set; }
    public virtual ICollection<Photo> Photos { get; set; }
    public virtual ICollection<Stock> Stocks { get; set; }
    public virtual ICollection<Price> Price { get; set; }
    public virtual Vendor Vendor { get; set; }
    [ConcurrencyCheck]
    [Timestamp]
    public byte[] RowVersion { get; set; }
    public bool Active { get; set; } = true;
    public Int64 ProductId { get; set; }

  }
}