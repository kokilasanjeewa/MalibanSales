using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateGrnHeaderDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "The SupplierInvoiceId cannot exceed 100 characters. ")]
        public string SupplierInvoiceId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SupplierInvoiceDate { get; set; }
        public int SupplierId { get; set; }
        public Int64? StoreId { get; set; }
        public int BranchLocationId { get; set; }
        [Required]
        public Int64 PurchaseOrderHeaderId { get; set; }
        [Required]
        public int ApplicationUserId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
        public virtual string CreatedBy { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalDiscountAmount { get; set; }

        /*
           [DataType(DataType.Currency)]
           [Column(TypeName = "decimal(18, 2)")]
           [DisplayFormat(DataFormatString = "{0:C}")]
           public decimal TotalBillAmount { get; set; } */

    }
}