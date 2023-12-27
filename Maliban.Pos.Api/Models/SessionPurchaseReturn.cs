using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  public class SessionPurchaseReturn : PurchaseReturn
  {
    public static PurchaseReturn GetPurchaseReturn(IServiceProvider services)
    {
      ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
      SessionPurchaseReturn purchase = session?.GetJson<SessionPurchaseReturn>("PurchaseReturn") ?? new SessionPurchaseReturn();
      purchase.Session = session;
      return purchase;
    }
    [JsonIgnore]
    public ISession Session { get; set; }
    /*    public override void AddHeder(SavePurchaseOrderHeaderDto purchaseOrderHeader){
         base.AddHeder(purchaseOrderHeader);
         Session.SetJson("PurchaseOrder", this);
       } */
    public override void AddItem(decimal quantity, Price price,[Optional] DateTime productExpireDate, Int64 transactionDetailId)
    {
      base.AddItem(quantity,price,productExpireDate,transactionDetailId);
      Session.SetJson("PurchaseReturn", this);
    }
    public override void RemoveLine(Product product, Price price)
    {
      base.RemoveLine(product,price);
      Session.SetJson("PurchaseReturn", this);
    }
    public override void Clear()
    {
      base.Clear();
      Session.Remove("PurchaseReturn");
    }
  }
}