using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MVCCivicaEmployeeMaster.Validations;

namespace MVCCivicaEmployeeMaster.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Salutation is required.")]
        [StringLength(5)]
        [DisplayName("Salutation")]
        public string Salutation { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50)]
        [MinLength(5)]
        [MaxLength(50)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Age")]
        [AgeOrDateOfBirthRequired(ErrorMessage = "Either Age or Date of Birth is required.")]
        [RegularExpression(@"^(1[8-9]|[2-9][0-9]|1[01][0-9]|120)$", ErrorMessage = "Age must be between 18 and 120.")]
        public int? Age { get; set; }

        [AgeRange(18, 120, ErrorMessage = "Date of birth must be between 18 and 120 years ago.")]
        [AgeOrDateOfBirthRequired(ErrorMessage = "Either Age or Date of Birth is required.")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(15)]
        [DisplayName("User name")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Emal address is required.")]
        [StringLength(50)]
        [DisplayName("Email Address")]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [StringLength(12)]
        [DisplayName("Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?:\+?\d{1,3})?(?:[-.\s]?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4})$", ErrorMessage = "Invalid contact number. The contact number must be between 10 and 12 digits long.")]

        public string Phone { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "The password must be at least 8 characters long and contain at least 1 uppercase letter, 1 number, and 1 special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required.")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Password hint is required.")]
        [Range(1, 3,ErrorMessage = "Please select a password hint")]
        public int PasswordHintId { get; set; }

        [Required(ErrorMessage = "PasswordHintAnswer is required.")]
        [StringLength(50)]
        [DisplayName("PasswordHintAnswer")]
        public string PasswordHintAnswer { get; set; }
    }
}
