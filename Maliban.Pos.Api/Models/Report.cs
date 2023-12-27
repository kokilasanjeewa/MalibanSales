using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Models
{
    public class Report: BaseModel
    {
        [Key]
        public int ReportId { get; set; }
        [Required]
        public string ReportName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReportCreateDate { get; set; }
        [Required]
        public string ReportPermisionUser { get; set; }

    }
}
