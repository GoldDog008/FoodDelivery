namespace Ipz_client.Models.Response
{
    public class DishResponseDto
    {
        public Guid DishId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }
}