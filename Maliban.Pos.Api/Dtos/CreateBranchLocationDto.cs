using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateBranchLocationDto
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
    [Display(Name = "Create By ")]
    public string CreatedBy { get; set; }

  }
}