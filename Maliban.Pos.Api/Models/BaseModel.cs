using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class BaseModel
  {
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; } = DateTime.Now;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifiedDate { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
    public virtual string CreatedBy { get; set; }
    [StringLength(100, ErrorMessage = "The ModifiedBy cannot exceed 100 characters. ")]
    public virtual string ModifiedBy { get; set; }
    public bool Active { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
  }
}