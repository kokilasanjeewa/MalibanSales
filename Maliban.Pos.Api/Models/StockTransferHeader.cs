using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Maliban.Pos.Api.Models
{
    public class StockTransferHeader : BaseModel
    {
        [Key]
        public Int64 StockTransferHeaderId { get; set; }
        [Required]
        public string StockTransferHeaderUid { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public int StockTransferHeaderFactoryId { get; set; }
        [Required]
        public string StockTransferHeaderReferenceNo { get; set; }
        [Required]
        public ICollection<StockTransferDetail> Lines { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalCost { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? TotalDiscountAmount { get; set; } = 0;
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]// maliban 18,2
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? TotalBillAmount { get; set; } = 0;
        public virtual Store Store { get; set; }
        [Required]
        public int BranchLocationId { get; set; }
        [ForeignKey("BranchLocationId")]
        public virtual BranchLocation BranchLocation { get; set; }
        [Required]
        public int TransferBranchLocationId { get; set; }
        [NotMapped]
        public virtual BranchLocation TransferBranchLocation { get; set; }

        [Required]
        [Display(Name = "ApplicationUser Id")]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int PaymentMethodId { get; set; }
        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        [Required]
        public bool IsApprove { get; set; } = false;
        [StringLength(100, ErrorMessage = "The ApprovedBy cannot exceed 100 characters. ")]
        public string ApprovedBy { get; set; }
        [Required]
        public bool IsComfirm { get; set; } = false;
        [StringLength(100, ErrorMessage = "The AuthorizedBy cannot exceed 100 characters. ")]
        public string AuthorizedBy { get; set; }
        [Required]
        public bool IsProcess { get; set; } = false;
        [StringLength(10, ErrorMessage = "The cApprovalType cannot exceed 10 characters. ")]
        public cApprovalType cApprovalType { get; set; } = 0;
        [Required]
        [StringLength(5, ErrorMessage = "The cDocType cannot exceed 5 characters. ")]
        public cDocType cDocType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateReceived { get; set; }
        [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
        public string ReceivedBy { get; set; }
  
    }

}
/*        [Required]
        public int SupplierId { get; set; }*/
/*        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        [ForeignKey("TransferSupplierId")]
        public virtual Supplier TransferSupplier { get; set; }
        [Required]
        public int TransferSupplierId { get; set; }*/