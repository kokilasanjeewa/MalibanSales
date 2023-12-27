using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Helper
{
  public sealed class EmployeeNumberValidate : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      string Message = string.Empty;
      string[] memberNames = new string[] { validationContext.MemberName };
      MI_Employer_Employee account = (MI_Employer_Employee)validationContext.ObjectInstance;

      if (account.EPFNo != value.ToString())
      {
        Message = "Incorrect employee number.Please check the employee number";
        return new ValidationResult(Message);
      }
      return ValidationResult.Success;
    }
  }
}