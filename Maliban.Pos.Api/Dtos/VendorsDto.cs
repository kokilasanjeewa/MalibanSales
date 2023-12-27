using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class VendorsDto
  {
    [Required]
    public int VendorId { get; set; }
    [Required]
    [Display(Name = "Vender Name")]
    [StringLength(50, ErrorMessage = "The Vendor Name cannot exceed 50 characters. ")]
    public string VendorName { get; set; }
  }
}