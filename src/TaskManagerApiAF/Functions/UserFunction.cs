using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using TaskManagerApiAF.Interfaces.IServices;
using TaskManagerApiAF.Models;
using TaskManagerApiAF.Utils;

namespace TaskManagerApiAF.Functions;

public class UserFunction
{
    private readonly ILogger<UserFunction> _logger;
    private readonly IConfiguration _config;
    private readonly IUserService _userService;

    public UserFunction(ILogger<UserFunction> logger, IConfiguration config, IUserService userService)
    {
        _logger = logger;
        _config = config;
        _userService = userService;
    }

    [Function("UserFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var authHeader = req.Headers.GetValues("Authorization")
            .FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return req.CreateResponse(HttpStatusCode.Unauthorized);

        string token = authHeader.Substring("Bearer ".Length).Trim();
        var principal = await TokenValidator.ValidateTokenAsync(token, _config["OpenId_Audience"]);

        var userData = new UsersTm();

        userData.ExternalId = principal.FindFirst("sub")?.Value ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        userData.Provider = _config["OpenId_Provider"];
        userData.Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? principal.FindFirst("email")?.Value;
        userData.DisplayName = principal.FindFirst("name")?.Value ?? principal.FindFirst("preferred_username")?.Value;
        userData.CreatedAt = DateTime.UtcNow;

        var createdUser = await _userService.CreateUser(userData);

        var resp = req.CreateResponse(HttpStatusCode.OK);
        await resp.WriteAsJsonAsync(new { createdUser.UserId, createdUser.Email, createdUser.DisplayName });
        return resp;
    }
}