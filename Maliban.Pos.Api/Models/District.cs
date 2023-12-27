using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class District : BaseModel
  {
    [Key]
    public int DistrictId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The District Name in English cannot exceed 50 characters. ")]
    public string DistrictNameEn { set; get; }
    [Required]
    [StringLength(50, ErrorMessage = "The District Name in Sinhala cannot exceed 50 characters. ")]
    public string DistrictNameSl { set; get; }
    [Required]
    [StringLength(50, ErrorMessage = "The District Name in Tamil cannot exceed 50 characters. ")]
    public string DistrictNameTm { set; get; }
    [Required]
    public int? ProvinceCode { set; get; }
    [Required]
    public int? DistrictCode { set; get; }
    [Required]
    public string DistrictUid { get; set; } = Guid.NewGuid().ToString();

  }
}