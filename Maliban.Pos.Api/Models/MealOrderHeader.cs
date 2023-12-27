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
    public class MealOrderHeader : BaseModel
    {
        [Key]
        public Int64 MealOrderHeaderId { get; set; }
        [Required]
        public string MealOrderHeaderUid { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Meal Order Create Date")]
        public DateTime MealOrderCreateDate { get; set; } = DateTime.Now;
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime MealOrderTimeStamp { get; set; } = DateTime.Now;
        [Required]
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        [Required]
        [Display(Name = "BranchLocation Id")]
        public int BranchLocationId { get; set; }
        [ForeignKey("BranchLocationId")]
        public virtual BranchLocation BranchLocation { get; set; }
        [Required]
        [Display(Name = "ApplicationUser Id")]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<MealOrderDetail> Lines { get; set; }
        [Required]
        public int MI_Employer_EmployeeId { get; set; }
        [Required]
        [StringLength(5)]
        public string EmployeeCode { get; set; }
        public virtual MI_Employer_Employee MI_Employer_Employee { get; set; }
        public MealType MealType { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Book Date From")]
        public DateTime BookDateFrom { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Book Date To")]
        public DateTime BookDateTo { get; set; }
    }
    public enum MealType { BreakFist=1, Lunch=2,Dinner=3}
}
