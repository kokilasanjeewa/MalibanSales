namespace Maliban.Pos.Api.Dtos
{
  public class CityListDto
  {
    public int CityId { get; set; }
    public int DistrictCode { get; set; }
    public string CityNameEn { get; set; }
    public string PostalCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }
}