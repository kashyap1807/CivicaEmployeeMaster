using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CivicaEmployeeMaster.Validations
{
    [ExcludeFromCodeCoverage]
    public class AgeRangeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public AgeRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == "" || value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Return success if the value is null or empty
                return ValidationResult.Success;
            }

            if (value is DateTime dateOfBirth)
            {
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;
                if (dateOfBirth.Date > today.AddYears(-age)) age--;

                if (age >= _minAge && age <= _maxAge)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"Age must be between {_minAge} and {_maxAge} years.");
                }
            }

            // Default validation error if value is not a DateTime
            return new ValidationResult("Date of birth is not valid.");
        }
    }

}
