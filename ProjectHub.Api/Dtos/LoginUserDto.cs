using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Api.Dtos
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
