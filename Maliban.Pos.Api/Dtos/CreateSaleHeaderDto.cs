using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maliban.Pos.Api.Dtos
{
  public class CreateSaleHeaderDto
  {
    [Required(ErrorMessage = "Please enter Customer.")]
    public string EmployeeCode { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Display(Name = "Order Date")]
    public DateTime OrderDate { get; set; } = DateTime.Now;
    [Required]
    [DataType(DataType.Time)]
    [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    //public DateTime OrderTimeStamp {  get{return DateTime.Now; } set{} }
    public DateTime OrderTimeStamp { get; set; } = DateTime.Now;
    [Required]
    public bool TransactStatus { get; set; } = false;
    public string ErrorLoc { get; set; }
    public string ErrorMessage { get; set; }
    public bool Paid { get; set; } = false;
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    //public DateTime PaymentDate {  get{return DateTime.Now; } set{} }
    public DateTime PaymentDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreateDate { get; set; } = DateTime.Now;
    //[Required]
    public int PaymentId { get; set; }
    [Required]
    [Display(Name = "BranchLocation Id")]
    public int BranchLocationId { get; set; }
    [Required]
    [Display(Name = "ApplicationUser Id")]
    public int Id { get; set; }
    //[Required]
    public ICollection<SaleDetail> Lines { get; set; }
    public string CreatedBy { get; set; }
    public string PaymentReferenceId { get; set; }
    public int cardType { get; set; } = 0;
    }
}