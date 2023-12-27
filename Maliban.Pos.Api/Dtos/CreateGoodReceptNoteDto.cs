using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateGoodReceptNoteDto
  {
    [Required]
    public string SupplierInvoiceId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime SupplierInvoiceDate { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal TotalCost { get; set; }
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal FreeItemsTotalCost { get; set; } = 0;
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal TotalDiscountAmount { get; set; } = 0;
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; } = DateTime.Now;
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public int StoreId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    [Display(Name = "ApplicationUser Id")]
    public int Id { get; set; }


  }
}