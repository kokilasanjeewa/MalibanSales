using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  public class SessionPurchaseRequest : PurchaseRequest
  {
    public static PurchaseRequest GetPurchaseRequest(IServiceProvider services)
    {
      ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
      SessionPurchaseRequest purchase = session?.GetJson<SessionPurchaseRequest>("PurchaseRequest") ?? new SessionPurchaseRequest();
      purchase.Session = session;
      return purchase;
    }
    [JsonIgnore]
    public ISession Session { get; set; }

    public override void AddItem(Product product, int quantity)
    {
      base.AddItem(product, quantity);
      Session.SetJson("PurchaseRequest", this);
    }
    public override void RemoveLine(Product product)
    {
      base.RemoveLine(product);
      Session.SetJson("PurchaseRequest", this);
    }
    public override void Clear()
    {
      base.Clear();
      Session.Remove("PurchaseRequest");
    }
  }
}