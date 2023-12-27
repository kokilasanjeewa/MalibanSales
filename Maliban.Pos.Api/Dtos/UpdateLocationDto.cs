using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateLocationDto: BaseDto
  {
    public string BranchCode { get; set; }
    public string BranchName { get; set; }
    public string Address { get; set; }
    public string ContactNumbers { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CountryId { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }
   
  }
}