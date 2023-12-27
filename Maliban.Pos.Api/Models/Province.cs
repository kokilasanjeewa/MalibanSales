using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Province : BaseModel
  {
    [Key]
    public int ProvinceId { get; set; }
    [Required(ErrorMessage = "The ProvinceName cannot exceed 50 characters. ")]
    public int? ProvinceCode { set; get; }
    [Required]
    [StringLength(50, ErrorMessage = "The Province Name in English cannot exceed 50 characters. ")]
    public string ProvinceNameEn { set; get; }
    [Required]
    [StringLength(50, ErrorMessage = "The Province Name in Sinhala cannot exceed 50 characters. ")]
    public string ProvinceNameSl { set; get; }
    [Required]
    [StringLength(50, ErrorMessage = "The Province Name in Tamil cannot exceed 50 characters. ")]
    public string ProvinceNameTm { set; get; }


    [Required]
    public string ProvinceUid { get; set; } = Guid.NewGuid().ToString();

  }
}