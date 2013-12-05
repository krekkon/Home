using System;
using System.ComponentModel.DataAnnotations;

namespace CarDealerProject.Models.Attributes
{
    public class CarYearAttribute : RangeAttribute
    {
        public CarYearAttribute(string minimumYear) 
            :base(typeof(DateTime), minimumYear, DateTime.Now.Year.ToString())
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var inputYear = value as int?;
            var result = (inputYear != null && inputYear >= 1886 && inputYear <= DateTime.Now.Year);

            return result ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
        }

        private static string GetErrorMessage()
        {
            return "The year must be greater than 1886 and lower than the actual year.";
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(GetErrorMessage(), name);
        }
    }
}