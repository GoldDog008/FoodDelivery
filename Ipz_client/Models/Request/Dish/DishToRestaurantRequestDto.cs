using System.ComponentModel.DataAnnotations;

namespace Ipz_client.Models.Request.Dish
{
    internal class DishToRestaurantRequestDto
    {
        [Required]
        public Guid RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 9999)]
        public decimal Price { get; set; }
    }
}
