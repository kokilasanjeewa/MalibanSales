using System.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Brand : BaseModel
  {
    [Key]
    public int BrandId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The BrandName cannot exceed 50 characters. ")]
    public string BrandName { set; get; }
    [StringLength(100, ErrorMessage = "The BrandDescription cannot exceed 100 characters. ")]
    public string BrandDescription { set; get; }
    [Required]
    public string BrandUid { get; set; } = Guid.NewGuid().ToString();
    public Int64? PhotoId { get; set; }
    public virtual Photo Photo { get; set; }
    public virtual ICollection<Product> Products { get; set; }
  }
}
