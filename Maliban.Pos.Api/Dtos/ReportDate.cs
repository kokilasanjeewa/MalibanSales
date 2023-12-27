using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class ReportDate
    {
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
        public int SchemeCode { get; set; }

    }
}
