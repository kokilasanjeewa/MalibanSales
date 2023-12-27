using System.Collections.Generic;
using System.Linq;


namespace Maliban.Pos.Api.Models
{
  public class PurchaseRequest
  {
    private List<PurchaseRequestDetail> lineCollection = new List<PurchaseRequestDetail>();
    public virtual void AddItem(Product product, int quantity)
    {
      PurchaseRequestDetail line = lineCollection
      .Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();

      if (line == null)
      {
        lineCollection.Add(new PurchaseRequestDetail
        {
          ProductId = product.ProductId,
          Quantity = quantity,
          Product = product,
        });
      }
      else
      {
        line.Quantity += quantity;
      }
    }
    public virtual void RemoveItem(Product product, int quantity)
    {
      PurchaseRequestDetail line = lineCollection
      .Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();
      if (line != null && line.Quantity > 0)
      {
        line.Quantity -= quantity;
      }
    }
    public virtual void RemoveLine(Product product) =>
    lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
    public virtual void Clear() => lineCollection.Clear();
    public virtual int Count => lineCollection.Count;
    public virtual ICollection<PurchaseRequestDetail> Lines => lineCollection;


  }

}
