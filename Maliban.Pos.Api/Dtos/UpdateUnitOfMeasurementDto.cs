using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateUnitOfMeasurementDto : BaseDto
  {
    public string UOM { get; set; }
    public string Description { get; set; }


  }
}