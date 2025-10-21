using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerApiAF.Interfaces.IRepositories;
using TaskManagerApiAF.Interfaces.IServices;
using TaskManagerApiAF.Services;
using TaskManagerApiAF.Utils;

namespace TaskManagementAF.Tests
{
    public class TokenAuthServiceTests
    {
        private readonly Mock<ITokenAuthService> _mockRepo;
        private readonly TokenAuthService _service;

        [Fact]
        public async Task ValidateAndSyncUserAsync_ValidToken_ReturnsPrincipal()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["OpenId_Audience"]).Returns("your-audience");
            mockConfig.Setup(c => c["OpenId_Authority"]).Returns("https://your-authority.com");

            var service = new TokenAuthService(mockConfig.Object);

            var validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."; // Usa un token real o simulado
            var principal = await service.ValidateAndSyncUserAsync(validToken);

            Assert.NotNull(principal);
            Assert.True(principal.Identity.IsAuthenticated);
        }

        [Fact]
        public async Task ValidateAndSyncUserAsync_InvalidToken_ReturnsNull()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["OpenId_Audience"]).Returns("your-audience");
            mockConfig.Setup(c => c["OpenId_Authority"]).Returns("https://your-authority.com");

            var service = new TokenAuthService(mockConfig.Object);
            var invalidToken = "invalid.token.value";

            var principal = await service.ValidateAndSyncUserAsync(invalidToken);
            Assert.Null(principal);
        }

    }
}