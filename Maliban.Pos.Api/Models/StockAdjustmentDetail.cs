using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
	public class StockAdjustmentDetail : BaseModel
	{
		[Key]
		public Int64 StockAdjustmentDetailId { get; set; }
		[Required]
		public string StockAdjustmentDetailUid { get; set; } = Guid.NewGuid().ToString();
		[Required]
		public Int64 StockAdjustmentHeaderId { get; set; }
		[Required]
		[StringLength(100, ErrorMessage = "The Barcode cannot exceed 100 characters. ")]
		public string Barcode { get; set; }
		[Required]
		[Display(Name = "Product Name")]
		[StringLength(100, ErrorMessage = "The ProductName cannot exceed 100 characters. ")]
		public string ProductName { get; set; }
		[Required]
		public decimal AdjustmentQuantity { get; set; }
		[Required]
		public decimal ApproveQuantity { get; set; } = 0;
		public decimal? CurrentQuantity { get; set; }
		public decimal? PreviousQuantity { get; set; }
		public virtual StockAdjustmentHeader StockAdjustmentHeader { get; set; }
		public virtual Product Product { get; set; }
		[ForeignKey("PriceId")]
		public virtual Price Price { get; set; }
		[Required]
		[Display(Name = "ProductId")]
		public Int64 ProductId { get; set; }
		[Required]
		[Display(Name = "PriceId")]
		public Int64 PriceId { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Display(Name = "Cost Price")]
		public decimal CostPrice { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Display(Name = "Retail Price")]
		public decimal RetailPrice { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Display(Name = "Stock Adjustment Total")]
		public decimal StockAdjustmentTotal { get; set; }
		[NotMapped]
		public string AdjustmentType { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime ProductExpireDate { get; set; }
		public Int64 TransactionDetailId { get; set; } = 0;
		[ForeignKey("TransactionDetailId")]
		public virtual TransactionDetail TransactionDetail { get; set; }
		[StringLength(10, ErrorMessage = "The cApprovalType cannot exceed 10 characters. ")]
		public cApprovalType cApprovalType { get; set; } = 0;
		}
		public enum cApprovalType { Approve=1, Reject=2, Cancel=3 }

}