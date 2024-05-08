using Ipz_server.Models.Dto.Orders;

namespace Ipz_server.Models.Dto
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string RestaurantName { get; set; } = null!;
        public List<OrderInformationResponseDto> OrderInformations { get; set; }
    }
}
