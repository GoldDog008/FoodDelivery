using Ipz_server.Models.Dto.Orders;
using Ipz_server.Models;

namespace Ipz_server.Services.IServices
{
    public interface IOrderService
    {
        public Task<ApiResponse> CreateOrder(Guid userId, OrderCreateRequestDto request);
        public Task<ApiResponse> GetAllOrders(Guid userId);
    }
}
