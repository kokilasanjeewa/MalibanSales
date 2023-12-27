using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class StockIssueDetail : BaseModel
  {
    [Key]
    public Int64 StockIssueDetailId { get; set; }
    [Required]
    public string StockIssueDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int IssueQuantity { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    public virtual Product Product { get; set; }
    [Required]
    public int BinId { get; set; }
    public virtual Bin Bin { get; set; }
    [Required]
    public Int64 GoodReceptNoteDetailId { get; set; }
    public virtual GoodReceptNoteDetail GoodReceptNoteDetail { get; set; }
    [Required]
    public Int64 StockIssueHeaderId { get; set; }
    public virtual StockIssueHeader StockIssueHeader { get; set; }
  }
}