using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CategoriesDto
  {
    [Required]
    public int CategoryId { get; set; }
    [Required]
    [Display(Name = "Category Name")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "The Category Name cannot exceed 100 characters. ")]
    public string CategoryName { get; set; }
  }
}