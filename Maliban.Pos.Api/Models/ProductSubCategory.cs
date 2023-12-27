using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class ProductSubCategory
  {
    [Key]
    public int ProductSubCategoryId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; }
  }
}