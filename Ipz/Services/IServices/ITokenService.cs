using Ipz_server.Models.Database;

namespace Ipz.Services.IServices
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
    }
}
