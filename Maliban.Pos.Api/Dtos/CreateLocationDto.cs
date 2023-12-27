using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateLocationDto
  {
    [Required]
    public string BranchCode { get; set; }
    [Required]
    public string BranchName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string ContactNumbers { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [Required]
    [Display(Name = "Create By ")]
    public string CreateBy { get; set; }

  }
}