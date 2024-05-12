using Ipz_client.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipz_client.Models
{
    public static class CurrentUser
    {
        public static Guid UserId { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string Email { get; set; }
        public static string Phone { get; set; }
        public static string Country { get; set; }
        public static string City { get; set; }
        public static string Street { get; set; }
        public static string AccessToken { get; set; }

        public static void SetCurrentUser(UserAuthResponseDto user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            Country = user.Country;
            City = user.City;
            Street = user.Street;
            AccessToken = user.AccessToken;
        }
    }
}
