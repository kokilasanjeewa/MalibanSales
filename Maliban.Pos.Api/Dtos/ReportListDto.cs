using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class ReportListDto
  {
    public int ReportId { get; set; }
    [Required]
    public string ReportName { get; set; }
    [Required]
    public string ReportPermisionUser { get; set; }
  }
}