using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class SectionListDto
  {
    [Required]
    public int SectionId { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "The Name cannot exceed 4 to 100 characters. ")]
    public string Name { get; set; }
  }
}