using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Models.Dto.Users
{
    public class UserUpdateRequestDto
    {
        [MinLength(2)]
        public string? FirstName { get; set; }
        [MinLength(2)]
        public string? LastName { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}
