using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  public class SessionSale : Sale
  {
    public static Sale GetSale(IServiceProvider services)
    {
      ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
      SessionSale sale = session?.GetJson<SessionSale>("Sale") ?? new SessionSale();
      sale.Session = session;
      return sale;
    }
    [JsonIgnore]
    public ISession Session { get; set; }

    public override void AddItem(Product product, decimal quantity, Price price, decimal stockQty,[Optional] bool activeBill,[Optional] decimal returnQuantity,[Optional] DateTime productExpireDate,Int64 transactionDetailId)
    {
      base.AddItem(product, quantity, price, stockQty,activeBill,returnQuantity,productExpireDate,transactionDetailId);
      Session.SetJson("Sale", this);
    }
    public override void UpdateQuantity(Product product, decimal quantity, decimal stockQty,Price price,Int64 transactionDetailId)
    {
      base.UpdateQuantity(product,quantity,stockQty,price,transactionDetailId);
      Session.SetJson("Sale",this);
    }
    public override void RemoveLine(Product product)
    {
      base.RemoveLine(product);
      Session.SetJson("Sale", this);
    }
    public override void Clear()
    {
      base.Clear();
      Session.Remove("Sale");
    }
  }
}