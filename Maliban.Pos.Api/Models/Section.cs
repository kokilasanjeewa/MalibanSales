using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Section : BaseModel
  {
    [Key]
    public int SectionId { get; set; }
    [Required]
    public string SectionUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50, ErrorMessage = "The Name cannot exceed 50 characters. ")]
    public string Name { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }


  }
}