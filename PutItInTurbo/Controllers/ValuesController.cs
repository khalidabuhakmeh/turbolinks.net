using Microsoft.AspNetCore.Mvc;
using TurboLinks.Net;

namespace PutItInTurbo.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        // Post
        [HttpPost, Route("")]
        public IActionResult Index()
        {
            return this.TurboLinkRedirectToPage("/Privacy");
        }
    }
}