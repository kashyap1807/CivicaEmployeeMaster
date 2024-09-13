using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CivicaEmployeeMaster.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "User name is required.")]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
