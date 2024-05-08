using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Ipz_server.Models.Dto.Orders
{
    public class OrderCreateRequestDto
    {
        [Required]
        public Guid RestaurantId { get; set; }
        [NotNull]
        public List<OrderInformationCreateRequestDto> OrderInformations { get; set; }
    }
}