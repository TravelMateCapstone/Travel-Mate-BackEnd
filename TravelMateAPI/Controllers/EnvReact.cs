using BusinessObjects.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvReact : Controller
    {
        private readonly AppSettings appSettings;

        public EnvReact(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult GetEnv()
        {
            var env = appSettings.ViteConfig;
            return Ok(env);
        }

    }
}
