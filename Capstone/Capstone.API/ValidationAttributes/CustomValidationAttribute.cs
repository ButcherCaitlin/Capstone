using System.ComponentModel.DataAnnotations;

namespace Capstone.API.ValidationAttributes
{
    public class CustomValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
