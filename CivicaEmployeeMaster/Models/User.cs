using System.ComponentModel.DataAnnotations;

namespace CivicaEmployeeMaster.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public string Salutation {  get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public PasswordHint PasswordHint { get; set; }

        public int PasswordHintId { get; set; }
        public string PasswordHintAnswer { get; set; }
    }
}
