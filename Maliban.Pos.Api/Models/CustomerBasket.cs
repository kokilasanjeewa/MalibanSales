using System.Collections.Generic;

namespace Maliban.Pos.Api.Models
{
  public class CustomerBasket
  {
    public CustomerBasket()
    {

    }
    public CustomerBasket(string id)
    {
      Id = id;
    }
    public string Id { get; set; }
    public string CustomerCode { get; set; }
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
  }
}