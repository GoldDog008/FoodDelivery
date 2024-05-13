using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipz_client.Models.Request.Order
{
    public class OrderInformationCreateRequestDto
    {
        public int Quantity { get; set; }
        public Guid DishId { get; set; }
    }
}
