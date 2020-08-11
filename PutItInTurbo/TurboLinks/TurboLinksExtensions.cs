using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TurboLinks.Net
{
    public static class TurboLinksExtensions
    {
        public static IApplicationBuilder 
            UseTurboLinks(this IApplicationBuilder app) => 
            app.UseMiddleware<TurboLinksMiddleware>();

        public static bool IsTurboLinkRequest(this HttpContext ctx) =>
            ctx.Request.Headers.ContainsKey("Turbolinks-Referrer");

        public static bool IsXhrRequest(this HttpContext ctx) =>
            ctx.Request.Headers.TryGetValue("X-Requested-With", out var result) && 
            result == "XMLHttpRequest";
        
        public static IActionResult TurboLinkRedirectToPage(this PageModel model, 
            string pageName,
            string pageHandler = null,
            object values = null,
            string protocol = null,
            TurboLinksActions turboLinksAction = TurboLinksActions.Active)
        {
            var url = model.Url.Page(pageName, pageHandler, values, protocol);
            return new TurbolinkRedirectResult(url, turboLinksAction);
        }
        
        public static IActionResult TurboLinkRedirectToAction(this PageModel target, 
            string action, 
            string controller,
            object values = null,
            TurboLinksActions turboLinksAction = TurboLinksActions.Active)
        {
            var url = target.Url.Action(action, controller, values);
            return new TurbolinkRedirectResult(url, TurboLinksActions.Active);
        }
        
        public static IActionResult TurboLinkRedirectToAction(this PageModel target, 
            string action, 
            object values = null,
            TurboLinksActions turboLinksAction = TurboLinksActions.Active)
        {
            var url = target.Url.Action(action, values);
            return new TurbolinkRedirectResult(url, turboLinksAction);
        }
        
        public static IActionResult TurboLinkRedirectToPage(this Controller model, 
            string pageName, 
            string pageHandler = null, 
            object values = null,
            string protocol = null,
            TurboLinksActions turboLinksAction = TurboLinksActions.Active)
        {
            var url = model.Url.Page(pageName, pageHandler, values, protocol);
            return new TurbolinkRedirectResult(url, turboLinksAction);
        }

        public static IActionResult TurboLinkRedirectToAction(this Controller target, 
            string action, 
            string controller,
            object values = null,
            TurboLinksActions turboLinksAction = TurboLinksActions.Active)
        {
            var url = target.Url.Action(action, controller, values);
            return new TurbolinkRedirectResult(url, turboLinksAction);
        }
        
        public static IActionResult TurboLinkRedirectToAction(this Controller target, 
            string action, 
            object values = null,
            TurboLinksActions turboLinksAction = TurboLinksActions.Active)
        {
            var url = target.Url.Action(action, values);
            return new TurbolinkRedirectResult(url, turboLinksAction);
        }
    }
}