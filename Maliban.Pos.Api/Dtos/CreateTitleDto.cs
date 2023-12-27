using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateTitleDto
  {
    [Required]
    [StringLength(50, ErrorMessage = "The TitleName cannot exceed 50 characters. ")]
    public string TitleName { set; get; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { set; get; }
    [Required]
    [StringLength(100, ErrorMessage = "The CreateBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
  }
}