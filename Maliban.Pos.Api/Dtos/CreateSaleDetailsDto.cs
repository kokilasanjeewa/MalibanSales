using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateSaleDetailDto
    {
        [Required]
        public int index { get; set; }
        public int itemcount { get; set; } = 0;
        [Required]
        public decimal quantity { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime billDate { get; set; }
        // [NotMapped]
        [Required]
        public int branchLocationId { get; set; }
        [Required]
        public string barCode { get; set; }
        public string number { get; set; }
        public int paymentId { get; set; } = 1;

    }

}