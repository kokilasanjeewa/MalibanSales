using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateVendorDto
  {
    [Required]
    [Display(Name = "Vender Name")]
    public string VendorName { get; set; }
    public string VendorDescription { get; set; }

    [Required]
    public string VendorCode { get; set; }
    [Required]
    public int? SupplierId { get; set; }
     [Required]
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }
  }
}