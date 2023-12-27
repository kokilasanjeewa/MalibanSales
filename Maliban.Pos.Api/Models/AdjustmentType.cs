using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class AdjustmentType : BaseModel
  {
    [Key]
    public int AdjustmentTypeId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The AdjustmentType cannot exceed 50 characters. ")]
    public string AdjustmentName { set; get; }
    [StringLength(100, ErrorMessage = "The AdjustmentName cannot exceed 100 characters. ")]
    public string AdjustmentDescription { set; get; }

    [Required]
    public string AdjustmentTypeUid { get; set; } = Guid.NewGuid().ToString();


  }
}