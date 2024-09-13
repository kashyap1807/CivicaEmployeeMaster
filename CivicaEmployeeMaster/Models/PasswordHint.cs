using System.ComponentModel.DataAnnotations;

namespace CivicaEmployeeMaster.Models
{
    public class PasswordHint
    {
        [Key]
        public int PasswordHintId { get; set; }
        public string PasswordHintQuestion { get; set; }
    }
}
