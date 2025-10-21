using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TaskManagerApiAF.Interfaces.IServices;

namespace TaskManagerApiAF.Utils
{
    public class TokenAuthService : ITokenAuthService
    {
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _configManager;
        private readonly IConfiguration _config;

        public TokenAuthService(IConfiguration config)
        {
            _config = config;
            var authority = _config["OpenId_Authority"];
            _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority.TrimEnd('/')}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());

        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string token, string validAudience)
        {           

            var openIdConfig = await _configManager.GetConfigurationAsync();
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = openIdConfig.Issuer,
                ValidAudiences = new[] { validAudience },
                IssuerSigningKeys = openIdConfig.SigningKeys,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                var principal = handler.ValidateToken(token, validationParameters, out validatedToken);
                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }

        }

        public async Task<ClaimsPrincipal?> ValidateAndSyncUserAsync(string token)
        {
            var audience = _config["OpenId_Audience"];
            return await ValidateTokenAsync(token, audience);
        }
    }
}
