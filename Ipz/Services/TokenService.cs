using Ipz.Services.IServices;
using Ipz_server.Models.Database;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Ipz.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _jwt;

        public TokenService(IConfiguration configuration)
        {
            _jwt = configuration.GetSection("JWT");
        }

        public string CreateAccessToken(User user)
        {
            try
            {
                using RSA rsa = RSA.Create();
                rsa.FromXmlString(_jwt.GetValue<string>("PrivateKey"));

                var securityKey = new RsaSecurityKey(rsa);

                var signingCredentials = new SigningCredentials(
                    key: securityKey,
                    algorithm: SecurityAlgorithms.RsaSha256)
                {
                    CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
                };

                var jwt = new JwtSecurityToken(
                    claims:
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, user?.Role.Name.ToString() ?? "Error"),
                    ],
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(int.Parse(_jwt.GetValue<string>("ExpirationTimeInMinutes"))),
                    signingCredentials: signingCredentials
                );

                var securityTokenHandler = new JwtSecurityTokenHandler();
                return securityTokenHandler.WriteToken(jwt);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating access token");
                throw;
            }
        }
    }
}
