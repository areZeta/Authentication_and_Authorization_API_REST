using System.ComponentModel.DataAnnotations;
namespace IdentityAPI.DTOs
{
    public class CredentialDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Not a valid Email.")]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Not a valid Password.")]
        //Password Length recommended by owasp (least 12 characters but not more than 64)
        [MaxLength(64, ErrorMessage = "Maxium 64 characters.")]
        [MinLength(12, ErrorMessage = "Maxium 12 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
