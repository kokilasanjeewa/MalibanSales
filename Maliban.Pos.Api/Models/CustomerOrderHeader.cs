using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class CustomerOrderHeader : BaseModel
  {
    [Key]
    public Int64 CustomerOrderHeaderId { get; set; }
    [Required]
    public string CustomerOrderHeaderUid { get; set; } = Guid.NewGuid().ToString();
    [StringLength(100, ErrorMessage = "The CustomerOrderNote cannot exceed 100 characters. ")]
    public string CustomerOrderNote { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CustomerOrderDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ExpectedDate { get; set; }
    [Required]
    public int BranchLocationId { get; set; }
    [Required]
    public bool IsComfirm { get; set; } = false;

    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Required]
    public decimal CustomerOrderAmount { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PaidAmount { get; set; }

    public virtual ICollection<CustomerOrderDetail> CustomerOrderDetails { get; set; }
    public virtual BranchLocation BranchLocation { get; set; }
    [Required]
    public int MI_Employer_EmployeeId { get; set; }
    [Required]
    public int EmployeeCode { get; set; }
    public virtual MI_Employer_Employee MI_Employer_Employee { get; set; }


  }
}