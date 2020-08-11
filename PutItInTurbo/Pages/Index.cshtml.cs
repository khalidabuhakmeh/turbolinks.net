using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Turbolinks.Net;

namespace PutItInTurbo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public bool HasName => !string.IsNullOrWhiteSpace(Name);
        public string Name { get; set; }

        public IActionResult OnPost()
        {
            return this.TurbolinksRedirectToPage("Privacy");
        }
    }
}