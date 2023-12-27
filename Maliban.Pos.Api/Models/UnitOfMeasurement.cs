using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class UnitOfMeasurement : BaseModel
  {
    [Key]
    public int UnitOfMeasurementId { get; set; }
    [Required]
    public string UnitOfMeasurementUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(10, ErrorMessage = "The UOM cannot exceed 100 characters. ")]
    public string UOM { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
    public string Description { get; set; }
  }
}