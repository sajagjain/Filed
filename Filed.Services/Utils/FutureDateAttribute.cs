using System;
using System.ComponentModel.DataAnnotations;

namespace Filed.Services.Utils
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateValue = (DateTime)value;
            if (dateValue >= DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Date Time cannot be less than Current Date and Time");
        }
    }
}
