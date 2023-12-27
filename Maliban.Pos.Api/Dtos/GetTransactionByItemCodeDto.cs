using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Maliban.Pos.Api.Models;
using static Maliban.Pos.Api.Models.Status;

namespace Maliban.Pos.Api.Dtos
{
    public class GetTransactionByItemCodeDto:BaseModel
    {
        public Int64 TransactionDetailId { get; set; }
        [Required]
        [Column("ProductName", Order = 4, TypeName = "nvarchar(100)")]
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReceivedDate      { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ReceivedUnitPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal UnitSellingprice { get; set; }
        public Decimal ReceivedQTY { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProductExpireDate { get; set; }
        public string EmployeeNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssuedDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal IssuingUnitPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitSelling { get; set; }
        public decimal IssuingQty { get; set; }
        public decimal Balance { get; set; }


    }
}