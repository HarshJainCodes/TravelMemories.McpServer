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
        public async Task RenderPermissionPage(
            [FromQuery] string client_id, 
            [FromQuery] string response_ype,
            [FromQuery] string code_challenge,
            [FromQuery] string code_challenge_method,
            [FromQuery] string scope,
            [FromQuery] string redirect_uri,
            [FromQuery] string state)
        {
            var html = @"
<html>
    <body>
        please grant permission
    </body>
</html>

            ";

            HttpContext.Response.ContentType = "text/html";
            await HttpContext.Response.WriteAsync(html);
        }
    }
}
