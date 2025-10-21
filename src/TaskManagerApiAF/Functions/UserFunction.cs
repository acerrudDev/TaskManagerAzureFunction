using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using TaskManagerApiAF.Helpers;
using TaskManagerApiAF.Interfaces.IServices;
using TaskManagerApiAF.Models;
using TaskManagerApiAF.Utils;

namespace TaskManagerApiAF.Functions;

public class UserFunction
{
    private readonly ILogger<UserFunction> _logger;
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly ITokenAuthService _tokenAuthService;

    public UserFunction(ILogger<UserFunction> logger, IConfiguration config, IUserService userService, ITokenAuthService tokenAuthService)
    {
        _logger = logger;
        _config = config;
        _userService = userService;
        _tokenAuthService = tokenAuthService;

    }

    [Function("UserFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processing a request.");

        return await AuthorizedFunctionExecutor.ExecuteAsync(
            req,
            _config,
            _tokenAuthService,
            async principal =>
            {
                var userData = new UsersTm
                {
                    ExternalId = principal.FindFirst("sub")?.Value ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Provider = _config["OpenId_Provider"],
                    Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? principal.FindFirst("email")?.Value,
                    DisplayName = principal.FindFirst("name")?.Value ?? principal.FindFirst("preferred_username")?.Value,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userService.CreateUser(userData);

                var resp = req.CreateResponse(HttpStatusCode.OK);
                await resp.WriteAsJsonAsync(new { createdUser.UserId, createdUser.Email, createdUser.DisplayName });
                return resp;

            }
        );

        
    }
}