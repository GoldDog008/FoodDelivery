namespace Ipz_server.Models.Dto
{
    public class DishResponseDto
    {
        public Guid DishId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }
}