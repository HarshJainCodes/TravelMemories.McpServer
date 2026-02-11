using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TravelMemories.McpServer.Controllers.AuthForVsCode
{
    [Route("authorize")]
    [ApiController]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult RenderPermissionPage(
            [FromQuery] string client_id, 
            [FromQuery] string response_type,
            [FromQuery] string code_challenge,
            [FromQuery] string code_challenge_method,
            [FromQuery] string scope,
            [FromQuery] string redirect_uri,
            [FromQuery] string state)
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "authorize.html"
            );

            return PhysicalFile(filePath, "text/html");
        }
    }
}
