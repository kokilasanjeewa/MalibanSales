using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Department : BaseModel
  {
    [Key]
    public int DepartmentId { get; set; }
    [Required]
    public string DepartmentUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "The Name cannot exceed 4 to 100 characters. ")]
    public string Name { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
  }
}