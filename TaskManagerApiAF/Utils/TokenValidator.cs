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

namespace TaskManagerApiAF.Utils
{
    public static class TokenValidator
    {
        private static ConfigurationManager<OpenIdConnectConfiguration> _configManager;

        public static void Init(string authority) 
        {
            _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority.TrimEnd('/')}/.well-known/openid-configuration", 
                new OpenIdConnectConfigurationRetriever());

        }

        public static async Task<ClaimsPrincipal> ValidateTokenAsync(string token, string validAudience)
        {
            if (_configManager == null)
                throw new InvalidOperationException("TokenValidator not initialized. Call Init(authority).");

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
            var principal = handler.ValidateToken(token, validationParameters, out validatedToken);
            return principal;
        }
    }
}
