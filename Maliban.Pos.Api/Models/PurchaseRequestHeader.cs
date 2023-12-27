using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Models
{
  public class PurchaseRequestHeader : BaseModel
  {
    [Key]
    public Int64 PurchaseRequestHeaderId { get; set; }
    [Required]
    public string PurchaseRequestHeaderUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int? BranchLocationId { get; set; }
    [Required]
    public int? DepartmentId { get; set; }
    [Required]
    public int? SectionId { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ExpectedDate { get; set; }
    [Required]
    public bool IsProcess { get; set; } = false;
    [Required]
    public bool IsComfirm { get; set; } = false;
    [Required]
    public virtual ICollection<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }
    [Required]
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public virtual PurchaseRequestStatus PurchaseRequestStatus { get; set; } = 0;
    [Required]
    public virtual Department Department { get; set; }
    [Required]
    public virtual Section Section { get; set; }
    [StringLength(100, ErrorMessage = "The ApprovedBy cannot exceed 100 characters. ")]
    public string ApprovedBy { get; set; }
    [StringLength(100, ErrorMessage = "The AuthorizedBy cannot exceed 100 characters. ")]
    public string AuthorizedBy { get; set; }

  }
}