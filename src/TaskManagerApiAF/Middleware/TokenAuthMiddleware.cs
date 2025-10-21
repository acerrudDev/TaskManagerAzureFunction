using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Interfaces.IServices;

namespace TaskManagerApiAF.Middleware
{
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenAuthService tokenValidator)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || 
                (!authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ?? true))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var principal = await tokenValidator.ValidateAndSyncUserAsync(token);

                if (principal == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                context.User = principal;
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authentication failed or Invalid Authorization Header.");
            }

        }
    }
}
