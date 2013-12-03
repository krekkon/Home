using System;
using System.ComponentModel.DataAnnotations;

namespace CarDealerProject.Models.Attributes
{
    public class CarYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputYear = value as int?;
            var result = (inputYear != null && inputYear >= 1886 && inputYear <= DateTime.Now.Year);
            return result;
        }
    }
}