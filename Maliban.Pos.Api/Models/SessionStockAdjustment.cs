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
  public class SessionStockAdjustment : StockAdjustment
  {
    public static StockAdjustment GetStockAdjustment(IServiceProvider services)
    {
      ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
      SessionStockAdjustment stockadjustment = session?.GetJson<SessionStockAdjustment>("StockAdjustment") ?? new SessionStockAdjustment();
      stockadjustment.Session = session;
      return stockadjustment;
    }
    [JsonIgnore]
    public ISession Session { get; set; }

    public override void AddItem(Product product, decimal quantity, Price price, decimal stockQty,string status,[Optional] DateTime productExpireDate, Int64 transactionDetailId, decimal approvequantity = 0, cApprovalType approvalType = 0)
    {
      base.AddItem(product, quantity, price, stockQty,status,productExpireDate,transactionDetailId,approvequantity,approvalType);
      Session.SetJson("StockAdjustment", this);
    }
    public override void UpdateQuantity(Product product, decimal quantity, decimal stockQty, Price price)
    {
      base.UpdateQuantity(product, quantity, stockQty, price);
      Session.SetJson("StockAdjustment", this);
    }
    public override void RemoveLine(Product product, Price price)
    {
      base.RemoveLine(product, price);
      Session.SetJson("StockAdjustment", this);
    }
    public override void Clear()
    {
      base.Clear();
      Session.Remove("StockAdjustment");
    }
  }
}