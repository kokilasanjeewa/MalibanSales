using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UpdateBinDto : BaseDto
  {
    public int? BranchLocationId { get; set; }
    public int? BinNumber { get; set; }
    public int? StoreId { set; get; }
    public int? StoreAreaId { set; get; }
    public int? ShelfNumber { set; get; }
    public int? RackId { get; set; }
    public string BinCode
    {
      get
      {
        return this.BranchLocationId + "-" + this.StoreId + "-" + this.StoreAreaId + "-" + this.RackId + "-" + this.ShelfNumber + "-" + this.BinNumber;

      }
    }
    public string BinDescription { set; get; }
   
  }
}