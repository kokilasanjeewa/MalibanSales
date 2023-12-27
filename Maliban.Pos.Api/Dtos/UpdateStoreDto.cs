using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateStoreDto: BaseDto
  {
    public string StoreName { get; set; }
    public string BranchLocationName { get; set; }
    public string StoreDescription { get; set; }
  }
}