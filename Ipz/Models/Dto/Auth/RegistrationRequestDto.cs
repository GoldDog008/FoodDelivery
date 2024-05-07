using System.ComponentModel.DataAnnotations;

namespace Ipz.Models.Dto.Auth
{
    public class RegistrationRequestDto
    {
        [MinLength(2)]
        public string FirstName { get; set; }
        [MinLength(2)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
