using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Models
{
  public class Category : BaseModel
  {
    [Key]
    [Display(Name = "Category Id")]
    public int CategoryId { get; set; }
    [Required]
    public string CategoryUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [Display(Name = "Category Name")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "The Category Name cannot exceed 100 characters. ")]
    public string CategoryName { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    public Int64? PhotoId { get; set; }
    public ICollection<ProductCategory> ProductCategory { get; set; }
    public ICollection<SubCategory> SubCategory { get; set; }
    public virtual Photo Photo { get; set; }
  }
}
