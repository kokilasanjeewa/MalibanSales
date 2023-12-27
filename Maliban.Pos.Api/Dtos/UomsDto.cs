using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class UomsDto
    {
    [Required]
    public int UnitOfMeasurementId { get; set; }
    [Required]
    [StringLength(10, ErrorMessage = "The UOM cannot exceed 100 characters. ")]
    public string UOM { get; set; }
    }
}