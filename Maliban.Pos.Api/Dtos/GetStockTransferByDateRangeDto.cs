using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class GetStockTransferByDateRangeDto : BaseModel
    {
        public Int64 StockTransferDetailId { get; set; }
        public string StockTransferHeaderReferenceNo  { get; set; }
        public string cDocNo  { get; set; }
        public string ProductCode { get; set; }
        [Required]
        [Column("ProductName", Order = 4, TypeName = "nvarchar(100)")]
        public string ProductName { get; set; }
        public string TransferBranch { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransferDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TransferUnitPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TransferSellingPrice { get; set; }
        public decimal TransferQty { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductExpireDate { get; set; }
        public string ReceiveBranch { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReceiveDate { get; set; }
        public string ReceiveBy { get; set; }
        public string TransferBy { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ReceiveUnitPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ReceiveSellingprice { get; set; }
        public Decimal ReceiveQty { get; set; }
        public string PaymentType { get; set; }
        public decimal Balance { get; set; }

    }
}

/*#
Product Core
Product Name
Branch
Doc#
Transfer Date
Unit Price
Unit Selling Price
Transfer QTY
Product Expire Date
Branch
Doc#
Received Date
Unit Price
Unit Selling
Received Qty
PaymentType
Balance*/
