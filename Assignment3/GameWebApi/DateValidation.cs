using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GameWebApi
{
    public class DateValidation : ValidationAttribute
    {
        public string GetErrorMessage() =>
        $"Incorrect date. Date should be current.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var givenDate = (DateTime)value;

            if (givenDate < DateTime.Today)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}