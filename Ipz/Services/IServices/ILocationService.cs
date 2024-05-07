using Ipz_server.Models.Dto.Locations;
using Ipz_server.Models;

namespace Ipz_server.Services.IServices
{
    public interface ILocationService
    {
        public Task<ApiResponse> CreateLocation(LocationCreateRequestDto request);
        public Task<ApiResponse> UpdateLocation(LocationUpdateRequestDto request);
    }
}
