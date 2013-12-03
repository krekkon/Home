using System;
using System.ComponentModel.DataAnnotations;

namespace CarDealerProject.Models.Attributes
{
    public class BirthDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputDate = value as DateTime?;
            var minDate = Convert.ToDateTime("01/01/1886");

            return (inputDate != null && minDate >= inputDate && inputDate <= DateTime.Now);
        }
    }
}