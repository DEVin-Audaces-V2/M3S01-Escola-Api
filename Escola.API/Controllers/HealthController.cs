using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Health()
        {
            return Ok("Its Working");
        }
    }
}
