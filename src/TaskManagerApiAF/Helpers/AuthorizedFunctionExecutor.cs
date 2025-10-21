using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Interfaces.IServices;

namespace TaskManagerApiAF.Helpers
{
    public static class AuthorizedFunctionExecutor
    {
        public static async Task<HttpResponseData> ExecuteAsync(
        HttpRequestData req,
        IConfiguration config,
        ITokenAuthService tokenService,
        Func<ClaimsPrincipal, Task<HttpResponseData>> onAuthorized)
        {
            if (!req.Headers.TryGetValues("Authorization", out var authHeaders))
                return req.CreateResponse(HttpStatusCode.Unauthorized);

            var authHeader = authHeaders.FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return req.CreateResponse(HttpStatusCode.Unauthorized);

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var principal = await tokenService.ValidateAndSyncUserAsync(token);

            if (principal == null)
                return req.CreateResponse(HttpStatusCode.Unauthorized);

            return await onAuthorized(principal);
        }

    }
}
