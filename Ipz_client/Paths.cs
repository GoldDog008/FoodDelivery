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
        public static string LoginPath = "/api/auth/login";
        public static string RegistrationPath = "/api/auth/register";

        //Dishes
        public static string GetDishesPath = "/api/dishes/{0}";
        public static string CreateDishPath = "/api/dishes";

        //Orders
        public static string GetOrdersPath = "/api/orders";


        //public static string GetCurrentUserPath = "/api/auth/getCurrentUser";
        //public static string UpdateCurrentUserPath = "/api/auth/updateCurrentUser";
        //public static string GetUsersPath = "/api/auth/getUsers";
    }
}
