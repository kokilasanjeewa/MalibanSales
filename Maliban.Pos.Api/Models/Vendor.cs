using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  public class Vendor : BaseModel
  {
    [Key]
    public int VendorId { get; set; }
    [Required]
    public string VendorUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [Display(Name = "Vender Name")]
    [StringLength(50, ErrorMessage = "The Vendor Name cannot exceed 50 characters. ")]
    public string VendorName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The Vendor Code cannot exceed 50 characters. ")]
    public string VendorCode { get; set; }
    [StringLength(100, ErrorMessage = "The Vendor Description cannot exceed 100 characters. ")]
    public string VendorDescription { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Product { get; set; }

  }
}