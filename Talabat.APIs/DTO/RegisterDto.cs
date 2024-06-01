using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTO
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%&amp;*()_+]).*$",ErrorMessage = "PassWord Must Be Contains 1 UpperCase , 1 LowerCase , 1 Digit , 1 Speacil Character !!!! ")]
        public string PassWord { get; set; }
    }
}
