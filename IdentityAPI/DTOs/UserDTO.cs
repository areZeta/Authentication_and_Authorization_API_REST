using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(40)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Not a valid Phone number.")]
        [MaxLength(10, ErrorMessage = "Only 10 digits.")]
        [MinLength(10, ErrorMessage = "Only 10 digits.")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Email { get; set; } = string.Empty;
    }
}
