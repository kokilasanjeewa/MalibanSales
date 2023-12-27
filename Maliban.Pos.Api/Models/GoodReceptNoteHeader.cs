using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Maliban.Pos.Api.Models
{
  public class GoodReceptNoteHeader : BaseModel
  {
    [Key]
    public Int64 GoodReceptNoteHeaderId { get; set; }
    [Required]
    public string GoodReceptNoteHeaderUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int GoodReceptNoteHeaderFactoryId { get; set; }
    [Required]
    public string GoodReceptNoteReferenceNo { get; set; }
    [Required]
    public ICollection<GoodReceptNoteDetail> Lines { get; set; }
    [Required]
    [StringLength(30, ErrorMessage = "The SupplierInvoiceId cannot exceed 100 characters. ")]
    public string SupplierInvoiceId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime SupplierInvoiceDate { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal TotalCost { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal FreeItemsTotalCost { get; set; }
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? TotalDiscountAmount { get; set; }=0;
        [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal? TotalBillAmount { get; set; }=0;
    public virtual Supplier Supplier { get; set; }
    public virtual Store Store { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual PurchaseOrderHeader PurchaseOrderHeader { get; set; }
    public ICollection<PurchaseReturnHeader> PurchaseReturnHeaders { get; set; }

    }

}
