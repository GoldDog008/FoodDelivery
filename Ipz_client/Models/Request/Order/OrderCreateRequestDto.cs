using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipz_client.Models.Request.Order
{
    public class OrderCreateRequestDto
    {
        public Guid RestaurantId { get; set; }
        public List<OrderInformationCreateRequestDto> OrderInformations { get; set; }
    }
}
