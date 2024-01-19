using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class ApproveStocktransferHeaderDto
    {
        [Required]
        public Int64 StockTransferHeaderId { get; set; }
        [Required]
        public cApprovalType cApprovalType { get; set; }
        [Required]
        public string createdBy { get; set; }
        public int id { get; set; }
    }
}
