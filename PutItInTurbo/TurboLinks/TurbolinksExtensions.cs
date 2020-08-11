using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Turbolinks.Net
{
    public static class TurbolinksExtensions
    {
        public static IApplicationBuilder 
            UseTurbolinks(this IApplicationBuilder app) => 
            app.UseMiddleware<TurbolinksMiddleware>();

        public static IServiceCollection
            AddTurbolinks(this IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<TurbolinksMiddleware>();

        public static bool IsTurbolinksRequest(this HttpContext ctx) =>
            ctx.Request.Headers.ContainsKey("Turbolinks-Referrer");

        public static bool IsXhrRequest(this HttpContext ctx) =>
            ctx.Request.Headers.TryGetValue("X-Requested-With", out var result) && 
            result == "XMLHttpRequest";
        
        public static IActionResult TurbolinksRedirectToPage(this PageModel model, 
            string pageName,
            string pageHandler = null,
            object values = null,
            string protocol = null,
            TurbolinksActions turbolinksAction = TurbolinksActions.Active)
        {
            var url = model.Url.Page(pageName, pageHandler, values, protocol);
            return new TurbolinksRedirectResult(url, turbolinksAction);
        }
        
        public static IActionResult TurbolinksRedirectToAction(this PageModel target, 
            string action, 
            string controller,
            object values = null,
            TurbolinksActions turbolinksAction = TurbolinksActions.Active)
        {
            var url = target.Url.Action(action, controller, values);
            return new TurbolinksRedirectResult(url, TurbolinksActions.Active);
        }
        
        public static IActionResult TurbolinksRedirectToAction(this PageModel target, 
            string action, 
            object values = null,
            TurbolinksActions turbolinksAction = TurbolinksActions.Active)
        {
            var url = target.Url.Action(action, values);
            return new TurbolinksRedirectResult(url, turbolinksAction);
        }
        
        public static IActionResult TurbolinksRedirectToPage(this Controller model, 
            string pageName, 
            string pageHandler = null, 
            object values = null,
            string protocol = null,
            TurbolinksActions turbolinksAction = TurbolinksActions.Active)
        {
            var url = model.Url.Page(pageName, pageHandler, values, protocol);
            return new TurbolinksRedirectResult(url, turbolinksAction);
        }

        public static IActionResult TurbolinksRedirectToAction(this Controller target, 
            string action, 
            string controller,
            object values = null,
            TurbolinksActions turbolinksAction = TurbolinksActions.Active)
        {
            var url = target.Url.Action(action, controller, values);
            return new TurbolinksRedirectResult(url, turbolinksAction);
        }
        
        public static IActionResult TurbolinksRedirectToAction(this Controller target, 
            string action, 
            object values = null,
            TurbolinksActions turbolinksAction = TurbolinksActions.Active)
        {
            var url = target.Url.Action(action, values);
            return new TurbolinksRedirectResult(url, turbolinksAction);
        }
    }
}