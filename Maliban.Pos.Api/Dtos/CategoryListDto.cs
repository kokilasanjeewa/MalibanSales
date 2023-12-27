using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class CategoryListDto
    {
    [Required]
    public int CategoryId { get; set; }
     [Required]
    [Display(Name = "Category Name")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "The Category Name cannot exceed 100 characters. ")]
    public string CategoryName { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [Required]
    public Int64 PhotoId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool Active { get; set; } 
    }
}