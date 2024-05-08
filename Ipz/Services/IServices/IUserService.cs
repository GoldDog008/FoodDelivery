using Ipz_server.Models.Dto.Users;
using Ipz_server.Models;
using Ipz_server.Models.Database;

namespace Ipz_server.Services.IServices
{
    public interface IUserService
    {
        public Task<ApiResponse> UpdateUserAsync(Guid id, UserUpdateRequestDto request);
        public Task<ApiResponse> GetUserByIdAsync(Guid id);
        public Task<ApiResponse> SaveUserToFile(User user);
        public Task<ApiResponse> IsAllDataFilledInAsync(Guid id);
    }
}
