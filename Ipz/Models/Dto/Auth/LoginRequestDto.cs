using System.ComponentModel.DataAnnotations;

namespace Ipz.Models.Dto.Auth
{
    public class LoginRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
    }
}
