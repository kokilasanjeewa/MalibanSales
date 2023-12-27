using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateVendorDto:BaseDto
  {
    [Required]
    [Display(Name = "Vender Name")]
    public string VendorName { get; set; }
    public string VendorDescription { get; set; }
    [Required]
    public string VendorCode { get; set; }
    [Required]
    public int? SupplierId { get; set; }
    public bool Active { get; set; }
    public bool IsDeleted { get; set; }
  }
}