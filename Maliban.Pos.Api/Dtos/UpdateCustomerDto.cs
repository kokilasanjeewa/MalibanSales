using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class UpdateCustomerDto
    {

        [Required]
        [StringLength(50)]
        public string Number { get; set; }
        //[Required]
        public int? Deleted { get; set; }
      
        //[Required]
        public DateTime DateOfAppointment { get; set; }
        [Required]
        public int? SchemeCode { get; set; }
        [Required]
        [StringLength(20)]
        public string EPFNo { get; set; }
        //[Required]
        public DateTime DateDeleted { get; set; }
        //[Required]
        [StringLength(100)]

        public string DeletedReason { get; set; }
         //[Required]
        public DateTime DateResigned { get; set; }
        [Required]
        [StringLength(2)]
        public string Location { get; set; }
           [Required]
        [StringLength(200)]
        public string FullName { get; set; }
       
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditLimit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditBalance { get; set; } = 0;
        public string Scheme { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PreviousCreditBalance { get; set; } = 0;
    }
}
