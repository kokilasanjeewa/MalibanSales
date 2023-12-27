using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdatePurchaseOrderHeaderDto
  {
    [Required]
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    public Int64 PurchaseOrderHeaderId { get; set; }
    [Required]

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DeliveryDate { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime PoDate { get; set; }
    [StringLength(30, ErrorMessage = "The QuatationNo cannot exceed 30 characters. ")]
    public string QuatationNo { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Display(Name = "Purchase Amount")]
    //  [Required]
    public decimal PurchaseAmount { get; set; }
    [Required]
    public int SupplierId { get; set; }
    public int? PaymentId { get; set; } = 0;
    [Required]
    public int? PaymentMethodId { get; set; }
    [StringLength(100, ErrorMessage = "The ApprovedBy cannot exceed 100 characters. ")]
    public string ApprovedBy { get; set; }
    [StringLength(100, ErrorMessage = "The AuthorizedBy cannot exceed 100 characters. ")]
    public string AuthorizedBy { get; set; }
   // [Required]
   // public string PurchaseOrderReferenceNo { get; set; }
    }
}