using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  public class PurchaseRequestDetail : BaseModel
  {
    [Key]
    public Int64 PurchaseRequestDetailId { get; set; }
    [Required]
    public string PurchaseRequestDetailUid { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public int Quantity { get; set; }
    [Required]
    public int RejectQuantity { get; set; } = 0;
    [Required]
    public int StockQuantity { get; set; } = 0;
    [Required]
    public int? BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    [Required]
    public Int64 PurchaseRequestHeaderId { get; set; }
    public virtual PurchaseRequestHeader PurchaseRequestHeader { get; set; }
    [Required]
    public int? UnitOfMeasurementId { get; set; }
    public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    public virtual Product Product { get; set; }
    // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [Required]
    public Int64? PurchaseOrderHeaderId { get; set; }
    public bool IsComfirm { get; set; } = false;
    public int ApprovedQuantity { get; set; } = 0;
    public virtual PurchaseOrderHeader PurchaseOrderHeader { get; set; }
  }
}