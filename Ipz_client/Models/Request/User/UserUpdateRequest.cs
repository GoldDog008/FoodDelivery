using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipz_client.Models
{
    public class UserUpdateRequest
    {
        [Required]
        [MinLength(2)]
        public string? FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string? LastName { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}
