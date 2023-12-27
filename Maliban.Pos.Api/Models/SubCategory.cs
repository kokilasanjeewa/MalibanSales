using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Models
{
  public class SubCategory : BaseModel
  {
    [Key]
    [Display(Name = "Category Id")]
    public int SubCategoryId { get; set; }
    [Required]
    public string SubCategoryUid { get; set; } = Guid.NewGuid().ToString();
    [Display(Name = "Category Name")]
    [StringLength(50, ErrorMessage = "The SubCategory Name cannot exceed 50 characters. ")]
    public string SubCategoryName { get; set; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
    [StringLength(450, ErrorMessage = "The PictureUrl cannot exceed 450 characters. ")]
    public string PictureUrl { get; set; }
    [Required]
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }
    public virtual ICollection<Product> Product { get; set; }
  }
}
