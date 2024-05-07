using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Models.Dto.Restaurants
{
    public class RestaurantCreateRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
    }
}
