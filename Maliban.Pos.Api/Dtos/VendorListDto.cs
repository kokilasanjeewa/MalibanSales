using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class VendorListDto
  {
    public int VendorId { get; set; }
    [Required]
    [Display(Name = "Vender Name")]
    public string VendorName { get; set; }
    [Required]
    public string VendorCode { get; set; }
    [Required]
    public int? SupplierId { get; set; }
    public bool Active { get; set; }
    public bool IsDeleted { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifyDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }

  }
}