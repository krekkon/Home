using System;
using System.ComponentModel.DataAnnotations;

namespace CarDealerProject.Models.Attributes
{
    public class BirthDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var inputDate = value as DateTime?;
            var minDate = Convert.ToDateTime("01/01/1886");
            var result = (inputDate != null && minDate <= inputDate && inputDate <= DateTime.Now);

            return result ? ValidationResult.Success : new ValidationResult("The date must be greater than 01/01/1886 and lower than the actual date.");
        }
    }
}