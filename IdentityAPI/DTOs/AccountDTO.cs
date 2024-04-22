using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.DTOs
{
    public class AccountDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(40)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Not a valid Phone number.")]
        [MaxLength(10, ErrorMessage = "Only 10 digits.")]
        [MinLength(10, ErrorMessage = "Only 10 digits.")]
        public string Phone { get; set; } = string.Empty;

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

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Required(ErrorMessage = "Password Confirmation is required.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Not a valid Password.")]
        [MaxLength(64, ErrorMessage = "Maxium 64 characters.")]
        [MinLength(12, ErrorMessage = "Maxium 12 characters.")]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}
