using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class UpdateSubCategoryDto
    {
    [Display(Name = "Category Name")]
    [StringLength(50, ErrorMessage = "The SubCategory Name cannot exceed 50 characters. ")]
    public string SubCategoryName { get; set; }
    
    }
}