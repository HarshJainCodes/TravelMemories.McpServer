using System.IdentityModel.Tokens.Jwt;

namespace TravelMemories.McpServer.Utilities
{
    public interface IRequestContextProvider
    {
        public JwtSecurityToken GetJWTToken();
    }
}
