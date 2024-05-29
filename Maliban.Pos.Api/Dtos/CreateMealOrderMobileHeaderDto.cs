using System.ComponentModel.DataAnnotations;
using System;

namespace Maliban.Pos.Api.Dtos
{
      public class CreateMealOrderMobileHeaderDto
    {
        [Required]
        public int BranchLocationId { get; set; }
        [Required]
        public int mohid { get; set; }
        [Required]
        public int MI_Employer_EmployeeId { get; set; }
        [Required]
        public int MealTypeId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Book Date From")]
        public DateTime ConfirmDateFrom { get; set; } = DateTime.Now.Date;
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public int PaymentId { get; set; } = 1;
        public string PaymentReferenceId { get; set; }
        [Required]  
        public int Id { get; set; } 
        public Int64 ProductId { get; set; }

    }

}
