using System.Threading.Tasks.Dataflow;
using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  //https://gist.github.com/adhipg/1600028
  public class Country : BaseModel
  {
    [Key]
    public int CountryId { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The Country Name cannot exceed 100 characters. ")]
    public string CountryName { set; get; }
    [StringLength(2, ErrorMessage = "The ISO Code cannot exceed 2 characters. ")]
    public string ISO { get; set; }
    [StringLength(100, ErrorMessage = "The Nice Name  cannot exceed 100 characters. ")]
    public string NiceName { get; set; }
    [StringLength(3, ErrorMessage = "The ISO3 Code cannot exceed 3 characters. ")]
    public string ISO3 { get; set; }
    [StringLength(5, ErrorMessage = "The Phone Code  cannot exceed 5 characters. ")]
    public string PhoneCode { get; set; }
    [Required]
    [StringLength(6, ErrorMessage = "The Country Code cannot exceed 6 characters. ")]
    public string CountryCode { set; get; }

    [Required]
    public string CountryUid { get; set; } = Guid.NewGuid().ToString();

  }
}