using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelMemories.McpServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TestingController : ControllerBase
    {
        [HttpGet]
        public async Task<string> TestController()
        {
            return "Working Successfully";
        }
    }
}
