using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class UserForLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required(ErrorMessage = "Preencha o password")]
        public string Password { get; set; }
    }
}