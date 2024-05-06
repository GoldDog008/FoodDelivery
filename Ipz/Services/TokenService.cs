using Ipz.Models.Database;
using Ipz.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Ipz.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _jwt;
        private readonly FoodDeliveryContext _context;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration,
            ILogger<TokenService> logger,
            FoodDeliveryContext context)
        {
            _jwt = configuration.GetSection("JWT");
            _logger = logger;
            _context = context;
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
                _logger.LogError(ex, "Error while creating access token");
                throw;
            }
        }
    }
}
