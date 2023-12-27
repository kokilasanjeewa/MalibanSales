using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Rack : BaseModel
  {
    [Key]
    public int RackId { get; set; }
    [Required]
    [StringLength(30, ErrorMessage = "The RackName cannot exceed 30 characters. ")]
    public string RackName { set; get; }
    [Required]
    public string RackUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(100, ErrorMessage = "The Rack Description cannot exceed 100 characters. ")]
    public string RackDescription { set; get; }
    public virtual StoreArea StoreArea { get; set; }
    public virtual ICollection<Bin> Bin { get; set; }
    [Required]
    public int NumberofShelves { get; set; }
  }
}