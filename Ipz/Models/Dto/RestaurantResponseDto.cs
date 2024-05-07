using Ipz_server.Models.Database;

namespace Ipz_server.Models.Dto
{
    public class RestaurantResponseDto
    {
        public Guid RestaurantId { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public virtual List<DishResponseDto> Dishes { get; set; } = new List<DishResponseDto>();
    }
}
