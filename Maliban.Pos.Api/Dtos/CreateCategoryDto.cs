using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateCategoryDto
  {
    [Required]
    [Display(Name = "Category Name")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "The Category Name cannot exceed 100 characters. ")]
    public string CategoryName { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The CreateBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
  }
}