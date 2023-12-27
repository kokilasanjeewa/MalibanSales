using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Models
{
  public class Payment : BaseModel
  {
    [Key]
    public int PaymentId { get; set; }
    [Required]
    public string PaymentUid { get; set; } = Guid.NewGuid().ToString();
    public PaymentType PaymentType { get; set; }
    public bool Allowed { get; set; }
    public virtual ICollection<SaleHeader> SaleHeader { get; set; }

  }
  public enum PaymentType { Credit,Cash }
}
