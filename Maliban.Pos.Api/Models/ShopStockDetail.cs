using System;

namespace Maliban.Pos.Api.Models
{
  public class ShopStockDetail
  {
    public Int64 ProductId { get; set; }
    public int dNetQuantity { get; set; }
    public decimal dSalePrice { get; set; }
    public string Barcode { get; set; }
    public string ProductName { get; set; }
  }
}