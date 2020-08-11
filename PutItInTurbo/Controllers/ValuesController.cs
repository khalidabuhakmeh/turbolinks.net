using Microsoft.AspNetCore.Mvc;
using Turbolinks.Net;

namespace PutItInTurbo.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        // Post
        [HttpPost, Route("")]
        public IActionResult Index()
        {
            return this.TurbolinksRedirectToPage("/Privacy");
        }
    }
}