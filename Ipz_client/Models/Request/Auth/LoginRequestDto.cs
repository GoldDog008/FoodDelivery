using System.ComponentModel.DataAnnotations;

namespace Ipz_client.Models.Request.Auth
{
    public class LoginRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
    }
}
