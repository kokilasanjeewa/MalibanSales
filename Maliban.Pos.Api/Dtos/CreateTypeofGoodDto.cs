using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateTypeofGoodDto
  {
    [Required]
    [StringLength(50, ErrorMessage = "The TypeofGoods cannot exceed 50 characters. ")]
    public string TypeofGoods { set; get; }
    [StringLength(100, ErrorMessage = "The GoodsDescription cannot exceed 100 characters. ")]
    public string GoodsDescription { set; get; }
    [Required]
    [StringLength(100, ErrorMessage = "The CreateBy cannot exceed 100 characters. ")]
    public string CreatedBy { get; set; }
  }
}