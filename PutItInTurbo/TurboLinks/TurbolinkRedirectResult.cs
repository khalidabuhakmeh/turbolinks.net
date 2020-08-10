using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace TurboLinks.Net
{
    public class TurbolinkRedirectResult : RedirectResult
    {
        public TurbolinkRedirectResult(string url) 
            : base(url)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var httpContext = context.HttpContext;
            if (httpContext.IsXhrRequest())
            {
                var content = httpContext.Request.Method == HttpMethods.Get
                    ? $"Turbolinks.visit('{this.Url}');"
                    : $"Turbolinks.clearCache();\nTurbolinks.visit('{this.Url}');";

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
}