using AutoMapper;
using Ipz_server.Models;
using Ipz_server.Models.Database;
using Ipz_server.Models.Dto;
using Ipz_server.Models.Dto.Orders;
using Ipz_server.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Services
{
    public class OrderService : IOrderService
    {
        private readonly FoodDeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public OrderService(FoodDeliveryContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ApiResponse> CreateOrder(Guid userId, OrderCreateRequestDto request)
        {
            List<OrderInformation> orderInformations = [];
            var apiResponse = new ApiResponse();
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Invalid model state");

                return apiResponse;
            }

            try
            {
                var isAllDataExists = await IsAllDataExists(userId, request);
                if (!isAllDataExists.Success)
                {
                    return isAllDataExists;
                }

                var isAllUserDataFilledInAsync = await _userService.IsAllDataFilledInAsync(userId);
                if (!isAllUserDataFilledInAsync.Success)
                {
                    return isAllUserDataFilledInAsync;
                }

                var isAllDishesExistsInRestaurant = await IsAllDishesExistsInRestaurant(request);
                if (!isAllDishesExistsInRestaurant.Success)
                {
                    return isAllDishesExistsInRestaurant;
                }

                var restaurant = isAllDishesExistsInRestaurant.Data as Restaurant;

                var dishes = await _context.Dishes
                    .Where(d => request.OrderInformations.Select(d => d.DishId).Contains(d.DishId))
                    .ToListAsync();

                var order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = userId,
                    Restaurant = restaurant,
                    CreatedAt = DateTime.Now,
                    TotalAmount = 0,
                };

                await _context.Orders.AddAsync(order);

                foreach (var dish in dishes)
                {
                    orderInformations.Add(new OrderInformation
                    {
                        OrderInformationsId = Guid.NewGuid(),
                        OrderId = order.OrderId,
                        DishId = dish.DishId,
                        Quantity = request.OrderInformations.FirstOrDefault(x => x.DishId == dish.DishId).Quantity,
                    });
                }

                order.TotalAmount = orderInformations.Sum(x => x.Quantity * dishes.FirstOrDefault(y => y.DishId == x.DishId).Price);

                await _context.OrderInformations.AddRangeAsync(orderInformations);
                await _context.SaveChangesAsync();

                var orderResponse = _mapper.Map<OrderResponseDto>(order);

                apiResponse.Success = true;
                apiResponse.Data = orderResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating order");
                throw;
            }

        }

        private async Task<ApiResponse> IsAllDishesExistsInRestaurant(OrderCreateRequestDto request)
        {
            var apiResponse = new ApiResponse();

            var restaurant = await _context.Restaurants
                                .Include(x => x.Dishes)
                                .FirstOrDefaultAsync(r => r.RestaurantId == request.RestaurantId);

            var isDishesExistsInRestaurant = request.OrderInformations
                .All(x => restaurant.Dishes.Select(d => d.DishId).Contains(x.DishId));

            if (!isDishesExistsInRestaurant)
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Dishes not found in restaurant");

                return apiResponse;
            }

            apiResponse.Success = true;
            apiResponse.Data = restaurant;
            return apiResponse;
        }

        private async Task<ApiResponse> IsAllDataExists(Guid userId, OrderCreateRequestDto request)
        {
            var apiResponse = new ApiResponse();
            var isRestaurantExists = await _context.Restaurants.AnyAsync(r => r.RestaurantId == request.RestaurantId);
            var isUserExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            var isDishesExists = await _context.Dishes
                .AnyAsync(d => request.OrderInformations.Select(d => d.DishId).Contains(d.DishId));

            if (!isRestaurantExists)
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Restaurant not found");

                return apiResponse;
            }

            if (!isUserExists)
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("User not found");

                return apiResponse;
            }

            if (!isDishesExists)
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Dishes not found");

                return apiResponse;
            }

            return apiResponse;
        }


        public async Task<ApiResponse> GetAllOrders(Guid userId)
        {
            var apiResponse = new ApiResponse();

            try
            {
                var orders = await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.OrderInformations)
                    .ThenInclude(d => d.Dish)
                    .Include(r => r.Restaurant)
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                if (orders == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("Orders not found");

                    return apiResponse;
                }

                List<OrderResponseDto> orderResponse = [];
                foreach (var order in orders)
                {
                    orderResponse.Add(_mapper.Map<OrderResponseDto>(order));
                }

                apiResponse.Success = true;
                apiResponse.Data = orderResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting order");
                throw;
            }
        }
    }
}
