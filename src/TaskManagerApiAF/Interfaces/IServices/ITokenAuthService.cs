using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApiAF.Interfaces.IServices
{
    public interface ITokenAuthService
    {
        Task<ClaimsPrincipal?> ValidateAndSyncUserAsync(string token);
    }
}
