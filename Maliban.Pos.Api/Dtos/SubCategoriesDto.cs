using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class SubCategoriesDto
  {
    [Required]
    [Display(Name = "Category Id")]
    public int SubCategoryId { get; set; }
    [Required]
    [Display(Name = "Category Name")]
    [StringLength(50, ErrorMessage = "The SubCategory Name cannot exceed 50 characters. ")]
    public string SubCategoryName { get; set; }
  }
}