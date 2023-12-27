using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateBrandDto: BaseDto
  {
    public string BrandName { set; get; }
    public string BrandDescription { set; get; }
  }
}