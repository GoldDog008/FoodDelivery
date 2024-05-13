namespace Ipz_client.Models
{
    public class OrderInformation
    {
        public int Quantity { get; set; }
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        override public string ToString()
        {
            return Name + ", " + Quantity + "шт., " + Price + "грн.";
        }
    }
}