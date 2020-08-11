using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace TurboLinks.Net
{
    public class TurbolinkRedirectResult : RedirectResult
    {
        public TurboLinksActions TurboLinksAction { get; }

        public TurbolinkRedirectResult(string url, TurboLinksActions turboLinksAction = TurboLinksActions.Active) 
            : base(url)
        {
            TurboLinksAction = turboLinksAction;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var httpContext = context.HttpContext;
            if (httpContext.IsXhrRequest())
            {
                var action = TurboLinksAction.ToString().ToLower();
                var content = httpContext.Request.Method == HttpMethods.Get
                    ? $"Turbolinks.visit('{this.Url}');"
                    : $"Turbolinks.clearCache();\nTurbolinks.visit('{this.Url}', {{ action: \"{ action }\" }});";

                var contentResult = new ContentResult {
                    Content = content,
                    ContentType = "text/javascript"
                };
                
                var executor = context
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<IActionResultExecutor<ContentResult>>();
                
                return executor.ExecuteAsync(context, contentResult);
            }
            else
            {
                return base.ExecuteResultAsync(context);                
            }
        }
    }

    public enum TurboLinksActions
    {    
        Active,
        Replace
    }
}