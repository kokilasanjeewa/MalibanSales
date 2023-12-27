using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Maliban.Pos.Api.Models
{
    public class MonthEndProcessHeader 
    {
        [Key]
        public Int64 MEPHeaderId { get; set; }
         [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
        [Required]
        [Display(Name = "BranchLocation Id")]
        public int BranchLocationId { get; set; }
        [ForeignKey("BranchLocationId")]
        public virtual BranchLocation BranchLocation { get; set; }
         public ICollection<MonthEndProcessDetail> Lines { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public decimal TotalReturnAmount { get; set; } = 0;
        public int SchemeCode { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Required]
        [StringLength(100, ErrorMessage = "The CreatedBy cannot exceed 100 characters. ")]
        public virtual string CreatedBy { get; set; }
    }

}

