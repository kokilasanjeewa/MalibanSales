using System;
using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
    public class SupplierPriceProductListDto
    {
    public virtual Product Product { get; set; }
    [Required]
    public Int64 ProductId { get; set; }
    }
}