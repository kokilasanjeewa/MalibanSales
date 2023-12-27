using System;
using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
  public class CreatePurchaseOrderDetailsDto
  {
    [Required]
    public string barCode { get; set; }
    [Required]
    public decimal quantity { get; set; }
    [Required]
    public decimal approveQuantity { get; set; } = 0;
    [Required]
    public cApprovalType cApprovalType { get; set; } = 0;
    [Required]
    public string createdBy { get; set; }
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
  //  [Required]
    public int SupplierPriceId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductExpiryDate { get; set; }
    }
}
 