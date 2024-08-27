using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateStockTransferHeaderDto
    {
        [Required]
        public int BranchLocationId { get; set; }
        [Required]
        public int TransferBranchLocationId { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public int paymentMethodId { get; set; }
        [Required]
        public string createdBy { get; set; }

    }
}
