using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateStoreAreaDto: BaseDto
  {
    public string StoreAreaName { get; set; }
    public string StoreAreaDescription { get; set; }
    public string BranchLocationName { get; set; }
    }
}