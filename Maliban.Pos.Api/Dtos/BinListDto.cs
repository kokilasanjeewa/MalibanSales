using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class BinListDto
  {
    public int BinId { get; set; }
    [Required]
    public string BranchName { get; set; }
    [Required]
    public string StoreName { set; get; }
    [Required]
    public string StoreAreaName { set; get; }
    [Required]
    public string RackName { set; get; }
    [Required]
    public string BinName { set; get; }
    [Required]
    public string BinDescription { set; get; }
    public bool Active { get; set; }
    public bool IsDeleted { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModifiedDate { get; set; }
    [Required]
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
  }
}