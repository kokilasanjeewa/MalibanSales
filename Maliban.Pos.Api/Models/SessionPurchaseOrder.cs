using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Models
{
  public class SessionPurchaseOrder : PurchaseOrder
  {
    public static PurchaseOrder GetPurchaseOrder(IServiceProvider services)
    {
      ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
      SessionPurchaseOrder purchase = session?.GetJson<SessionPurchaseOrder>("PurchaseOrder") ?? new SessionPurchaseOrder();
      purchase.Session = session;
      return purchase;
    }
    [JsonIgnore]
    public ISession Session { get; set; }
    /*    public override void AddHeder(SavePurchaseOrderHeaderDto purchaseOrderHeader){
         base.AddHeder(purchaseOrderHeader);
         Session.SetJson("PurchaseOrder", this);
       } */
    public override void AddItem(decimal quantity, SupplierPrice supplierPrice, DateTime ProductExpiryDate,decimal approvequantity = 0, cApprovalType approvalType = 0)
    {
      base.AddItem(quantity, supplierPrice, ProductExpiryDate,approvequantity, approvalType);
      Session.SetJson("PurchaseOrder", this);
    }
    public override void RemoveLine(Product product, SupplierPrice supplierPrice, DateTime ProductExpiryDate)
    {
      base.RemoveLine(product,supplierPrice, ProductExpiryDate);
      Session.SetJson("PurchaseOrder", this);
    }
    public override void Clear()
    {
      base.Clear();
      Session.Remove("PurchaseOrder");
    }
  }
}