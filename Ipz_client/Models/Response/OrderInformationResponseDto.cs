﻿namespace Ipz_client.Models.Response
{
    public class OrderInformationResponseDto
    {
        public string DishName { get; set; } = null!;
        public int Quantity { get; set; }

        override public string ToString()
        {
            return $"{DishName}, {Quantity} шт.";
        }
    }
}
