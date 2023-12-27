using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class BrandsDto
  {
    [Required]
    public int BrandId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The BrandName cannot exceed 50 characters. ")]
    public string BrandName { set; get; }
  }
}