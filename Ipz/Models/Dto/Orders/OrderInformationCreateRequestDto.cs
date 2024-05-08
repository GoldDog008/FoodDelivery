using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Models.Dto.Orders
{
    public class OrderInformationCreateRequestDto
    {
        [Range(1, 99)]
        public int Quantity { get; set; }
        [Required]
        public Guid DishId { get; set; }
    }
}