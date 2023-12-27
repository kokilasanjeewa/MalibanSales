using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateStockAdjustmentDetailDto
  {
    [Required]
    public string barcode { get; set; }
    [Required]
    public int stockAdjustmentQuantity { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime stockAdjustmentDate { get; set; }=DateTime.Now;
    // [NotMapped]
    [Required]
    public int branchLocationId { get; set; }
    //[Column(TypeName = "decimal(18, 2)")]
    //[DisplayFormat(DataFormatString = "{0:C}")]
    //public decimal costPrice { get; set; } 
    // [Column(TypeName = "decimal(18, 2)")]
    // [DisplayFormat(DataFormatString = "{0:C}")]
    // public decimal salePrice { get; set; }
    //[Required]
    //public decimal dCostPrice { get; set; }

    }
}