using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PutItInTurbo.Pages
{
    public class Redirect : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("Index", new {time = DateTime.Now});
        }
    }
}