using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipz_client
{
    public static class Paths
    {
        public static string Host = "https://localhost:7067";

        //Auth
        public static string Login = Host + "/api/auth/login";
        public static string Registration = Host + "/api/auth/register";

        //Dishes
        public static string GetDishes = Host + "/api/dishes/{0}";
        public static string CreateDish = Host + "/api/dishes";

        //Orders
        public static string GetOrders = Host + "/api/orders";
        public static string CreateOrder = Host + "/api/orders";

        //Restaurants
        public static string GetRestaurants = Host + "/api/restaurants";
        public static string GetRestaurant = Host + "/api/restaurants/{0}";
        public static string CreateRestaurant = Host + "/api/restaurants";

        //Users
        public static string GetUser =  Host + "/api/users/{0}";
        public static string UpdateUser = Host + "/api/users/";      

        //Polling
        public static string GetCurrentTime = Host + "/api/polling";
    }
}
