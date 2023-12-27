using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateDepartmentDto
    {
    [Required]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "The Name cannot exceed 4 to 100 characters. ")]
    public string Name { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; } 
        [Required]
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
    }
}