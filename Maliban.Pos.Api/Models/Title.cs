using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Title : BaseModel
  {
    [Key]
    public int TitleId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The TitleName cannot exceed 50 characters. ")]
    public string TitleName { set; get; }
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { set; get; }
    [Required]
    public string TitleUid { get; set; } = Guid.NewGuid().ToString();
  }
}