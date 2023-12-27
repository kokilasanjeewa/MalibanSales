using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
		public class PurchaseOrderDetail
		{
				[Key]
				public Int64 PurchaseOrderDetailId { get; set; }
				[Required]
				public string PurchaseOrderDetailUid { get; set; } = Guid.NewGuid().ToString();
				[Required]
				public Int64 PurchaseOrderHeaderId { get; set; }
				[Required]
				public Int64? ProductId { get; set; }
				[Required]
				public Int64 SupplierPriceId { get; set; }
				[Required]
				[DataType(DataType.Date)]
				[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
				public DateTime CreateDate { get; set; } = DateTime.Now;
				[DataType(DataType.Date)]
				[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
				public DateTime ModifiedDate { get; set; }
				// [Required]
				[StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
				public string CreatedBy { get; set; }
				[StringLength(100, ErrorMessage = "The ModifiedBy cannot exceed 100 characters. ")]
				public string ModifiedBy { get; set; }
				public bool Active { get; set; } = true;
				public bool IsDeleted { get; set; } = false;

				[DataType(DataType.Date)]
				[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
				public DateTime ModifyDate { get; set; }
				[Required]
				public decimal Quantity { get; set; }
				[Required]
				public decimal ApproveQuantity { get; set; } = 0;

				/*  [Required]
					[Column(TypeName = "decimal(18, 2)")]
					[DisplayFormat(DataFormatString = "{0:C}")]
					[Display(Name = "Order Price")]
					public decimal? OrderPrice { get; set; } */
				[Required]
				[Column(TypeName = "decimal(18, 2)")]
				[DisplayFormat(DataFormatString = "{0:C}")]
				[Display(Name = "Amount")]
				public decimal Amount { get; set; }
				[Required]
				public decimal? CurrentQuantity { get; set; } = 0;
				public decimal? ReturnQuantity { get; set; } = 0;
				public decimal? BonusQuantity { get; set; } = 0;
				public decimal? GRNQuantity { get; set; } = 0;
				public decimal? FreeQuantity { get; set; } = 0;
				[Required]
				[DataType(DataType.Date)]
				[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
				public DateTime ProductExpiryDate { get; set; }
				public virtual PurchaseOrderHeader PurchaseOrderHeaders { get; set; }
				public virtual Product Product { get; set; }
				public virtual SupplierPrice SupplierPrice { get; set; }
				[StringLength(10, ErrorMessage = "The cApprovalType cannot exceed 10 characters. ")]
				public cApprovalType cApprovalType { get; set; } = 0;
		}
}