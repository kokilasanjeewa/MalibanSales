using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
	public class PurchaseOrderHeader : BaseModel
	{
		[Key]
		public Int64 PurchaseOrderHeaderId { get; set; }
		[Required]
		public string PurchaseOrderUid { get; set; } = Guid.NewGuid().ToString();
		[Required]
		public string PurchaseOrderReferenceNo { get; set; }
		[StringLength(100, ErrorMessage = "The Description cannot exceed 100 characters. ")]
		public string Description { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime PoDate { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "DeliveryDate")]
		public DateTime DeliveryDate { get; set; }
		[Required]
		public bool IsProcess { get; set; } = false;
		[StringLength(30, ErrorMessage = "The QuatationNo cannot exceed 30 characters. ")]
		public string QuatationNo { get; set; }
		[Required]
		public bool IsComfirm { get; set; } = false;
		[Column(TypeName = "decimal(18, 2)")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Display(Name = "Purchase Amount")]
		[Required]
		public decimal PurchaseAmount { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Display(Name = "Paid Amount")]
		public decimal PaidAmount { get; set; }
		public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails
		{
			get; set;

		}
		[Required]
		public int BranchLocationId { get; set; }
		public virtual BranchLocation BranchLocation { get; set; }
		[Required]
		public int SupplierId { get; set; }
		public virtual Supplier Supplier { get; set; }
		[Required]
		public int? PaymentMethodId { get; set; }
		public virtual PaymentMethod PaymentMethod { get; set; }
		public int? PaymentId { get; set; }
		public virtual Payment Payment { get; set; }
		[StringLength(100, ErrorMessage = "The ApprovedBy cannot exceed 100 characters. ")]
		public string ApprovedBy { get; set; }
		[StringLength(100, ErrorMessage = "The AuthorizedBy cannot exceed 100 characters. ")]
		public string AuthorizedBy { get; set; }
		/*     public virtual ICollection<PurchaseReturnHeader> PurchaseReturnHeaders { get; set; }
				public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; } */
		[Required]
		public virtual ICollection<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }
		[Required]
		public bool IsApprove { get; set; } = false;
		[Required]
		[Display(Name = "ApplicationUser Id")]
		public int Id { get; set; }
		[ForeignKey("Id")]
		public virtual ApplicationUser ApplicationUser { get; set; }
		[StringLength(10, ErrorMessage = "The cApprovalType cannot exceed 10 characters. ")]
		public cApprovalType cApprovalType { get; set; } = 0;

		/* 	USE [MalibanPosDB]
	GO

	UPDATE [dbo].[PurchaseOrderHeaders]
	   SET [dbo].[PurchaseOrderHeaders].Id=(select u.Id from AspNetUsers u where u.UserName like [dbo].[PurchaseOrderHeaders].CreatedBy)

	GO */

	}
}