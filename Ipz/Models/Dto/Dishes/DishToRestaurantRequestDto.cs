using Ipz_server.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Models.Dto.Dishes
{
    public class DishToRestaurantRequestDto
    {
        [Required]
        public Guid RestaurantId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, 9999)]
        public decimal Price { get; set; }
    }
}