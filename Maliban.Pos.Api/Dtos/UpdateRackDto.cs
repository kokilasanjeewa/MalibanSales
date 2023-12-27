using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateRackDto: BaseDto
  {
    public string RackName { set; get; }
    public string RackDescription { set; get; }
    }
}