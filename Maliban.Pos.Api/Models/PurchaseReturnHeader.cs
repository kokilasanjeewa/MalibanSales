using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class PurchaseReturnHeader : BaseModel
  {
    [Key]
    public Int64 PurchaseReturnHeaderId { get; set; }
    [Required]
    public string PurchaseReturnHeaderUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }

    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime PurchaseReturnDate { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PurchaseReturnAmount { get; set; }
    public virtual ICollection<PurchaseReturnDetail> Lines { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public int ReturnTypeId { get; set; }
    [ForeignKey("ReturnTypeId")]
    public virtual ReturnReason ReturnReason { get; set; }
    [Required]
    public Int64 GoodReceptNoteHeaderId { get; set; }
    [ForeignKey("GoodReceptNoteHeaderId")]
    public virtual GoodReceptNoteHeader GoodReceptNoteHeader { get; set; }
    }
}