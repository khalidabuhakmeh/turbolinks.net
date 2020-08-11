using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Turbolinks.Net
{
    public class TurbolinksRedirectResult : RedirectResult
    {
        public TurbolinksActions TurbolinksAction { get; }

        public TurbolinksRedirectResult(string url, TurbolinksActions turbolinksAction = TurbolinksActions.Active) 
            : base(url)
        {
            TurbolinksAction = turbolinksAction;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var httpContext = context.HttpContext;
            if (httpContext.IsXhrRequest())
            {
                var action = TurbolinksAction.ToString().ToLower();
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

    public enum TurbolinksActions
    {    
        Active,
        Replace
    }
}