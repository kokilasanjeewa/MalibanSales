using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class RequestForQuotationHeader : BaseModel
  {
    [Key]
    public Int64 RequestForQuotationHeaderId { get; set; }
    [Required]
    public string RequestForQuotationHeaderUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int BranchLocationId { get; set; }
    [StringLength(200, ErrorMessage = "The Description cannot exceed 200 characters. ")]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ExpectedDate { get; set; }
    [Required]
    public bool IsProcess { get; set; } = false;
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public bool IsComfirm { get; set; } = false;
    [Required]
    public string SupplierName { get; set; }
    [Required]
    public virtual ICollection<RequestForQuotationDetail> RequestForQuotationDetail { get; set; }
    [Required]
    public virtual Supplier Supplier { get; set; }
    [Required]
    public virtual BranchLocation BranchLocation { get; set; }
    [StringLength(100, ErrorMessage = "The ApprovedBy cannot exceed 100 characters. ")]
    public string ApprovedBy { get; set; }
    [StringLength(100, ErrorMessage = "The AuthorizedBy cannot exceed 100 characters. ")]
    public string AuthorizedBy { get; set; }
  }
}