using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateMealOrderHeaderDto
    {
        [Required]
        public int PaymentId { get; set; }
        [Required]
        public int BranchLocationId { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public int MI_Employer_EmployeeId { get; set; }
        [Required]
        [StringLength(5)]
        public string EmployeeCode { get; set; }
       // [NotMapped]
        [Required]
        public int MealTypeId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Book Date From")]
        public DateTime BookDateFrom { get; set; } = DateTime.Now.Date;
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Book Date To")]
        public DateTime BookDateTo { get; set; } = DateTime.Now.Date;
        // [NotMapped]
        [Required]
        public string List { get; set; }  
        public string CreatedBy { get; set; }
        public string PaymentReferenceId { get; set; }
    }
     public class CartItem
    {
        [Required]
        public decimal price { get; set; }
        public Int64 productId { get; set; }
        [Required]
        public Int64 priceId { get; set; }
        [Required]
        public int quantity { get; set; }
        public string src { get; set; }
        public string alt { get; set; }
        public string menuitemheading { get; set; }
    }
}
