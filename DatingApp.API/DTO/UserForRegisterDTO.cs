using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Minimal Length is 3.")]
        public string Password { get; set; }
    }
}