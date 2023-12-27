using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
    public class GetBinByItemCodeDto : BaseModel
    {
        public Int64 TransactionDetailId { get; set; }
        public Int64 tSerialNo { get; set; }
        [Required]
        [Column("ProductName", Order = 4, TypeName = "nvarchar(100)")]
        public string ProductName { get; set; }
        public Int64 tRefId { get; set; }
        public Int64 SaleDetailId { get; set; }
        public string ProductCode { get; set; }
        public string tType { get; set; }
        public cDocType tiType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReceivedDate      { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ReceivedUnitPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal UnitSellingprice { get; set; }
        public Decimal ReceiveQty { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductExpireDate { get; set; }
        public string EmployeeNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssuedDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal IssuedUnitPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal IssuedSoldPrice { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal tNetQty { get; set; }
        public decimal Balance { get; set; }
        public string PaymentType { get; set; }
    }
}