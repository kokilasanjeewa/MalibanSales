using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  public class City : BaseModel
  {
    [Key]
    public int CityId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The City Name English cannot exceed 50 characters. ")]
    [Column("Name In English", Order = 1)]
    public string CityNameEn { set; get; }
    [Required]
    [StringLength(50, ErrorMessage = "The City Name Sinhala cannot exceed 50 characters. ")]
    [Column("Name In Sinhala", Order = 2)]
    public string CityNameSl { set; get; }
    [Required]
    [StringLength(50, ErrorMessage = "The City Name Tamil cannot exceed 50 characters. ")]
    [Column("Name In Tamil", Order = 3)]
    public string CityNameTm { set; get; }
    [Required]
    [StringLength(5, ErrorMessage = "The Postal Code cannot exceed 5 characters. ")]
    [Column("Postal Code")]
    public string PostalCode { set; get; }
    [Column("Latitude")]
    public double Latitude { set; get; }
    [Column("Longitude")]
    public double Longitude { set; get; }
    [Required]
    public string CityUid { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public int? DistrictCode { get; set; }
  }
}