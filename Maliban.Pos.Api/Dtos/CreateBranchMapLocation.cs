using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateBranchMapLocation
    {
        [Required]
        [Display(Name = "branchCode")]
        public int branchCodeId { get; set; }
        [Required]
        [Display(Name = "epfNo")]
        public string epfNo { get; set; }
        [Required]
        [Display(Name = "tomapBranchCode")]
        public int tomapBranchCodeId { get; set; }
    }
}
