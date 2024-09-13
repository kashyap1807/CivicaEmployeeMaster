using MVCCivicaEmployeeMaster.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MVCCivicaEmployeeMaster.Validations
{
    [ExcludeFromCodeCoverage]
    public class AgeOrDateOfBirthRequiredAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (RegisterViewModel)validationContext.ObjectInstance;

            if (!model.Age.HasValue && model.DateOfBirth == null)
            {
                return new ValidationResult("Either Age or Date of Birth is required.");
            }

            return ValidationResult.Success;
        }
    }
}
