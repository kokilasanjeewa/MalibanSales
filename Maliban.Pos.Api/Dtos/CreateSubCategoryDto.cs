using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateSubCategoryDto
  {
    [Required]
    [Display(Name = "Category Name")]
    [StringLength(50, ErrorMessage = "The SubCategory Name cannot exceed 50 characters. ")]
    public string SubCategoryName { get; set; }
    [Required]
    public int? CategoryId { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
  }
}