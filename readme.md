# TurboLinks .NET

This is a sample [ASP.NET Core](https://asp.net) project working with [TurboLinks 5](https://github.com/turbolinks/turbolinks).

> Turbolinks® makes navigating your web application faster. Get the performance benefits of a single-page application without the added complexity of a client-side JavaScript framework. Use HTML to render your views on the server side and link to pages as usual. When you follow a link, Turbolinks automatically fetches the page, swaps in its &lt;body&gt;, and merges its &lt;head&gt;, all without incurring the cost of a full page load.

## TurboLinksMiddleware

The `TurboLinksMiddleware` adds a `Turbolinks-Location` header to responses, allowing TurboLinks the ability to manage **visits** of the client.

```c#
public class TurboLinksMiddleware : IMiddleware
{
    public const string TurbolinksLocationHeader 
        = "Turbolinks-Location";

    public async Task InvokeAsync(
        HttpContext httpContext, 
        RequestDelegate next
    )
    {
        httpContext.Response.OnStarting((state) => {
            if (state is HttpContext ctx) 
            {
                if (ctx.IsTurboLinkRequest())
                {
                    ctx.Response.Headers.Add(TurbolinksLocationHeader, ctx.Request.GetEncodedUrl());
                }
            }

            return Task.CompletedTask;
        }, httpContext);
        
        await next(httpContext);
    }
}
```

## TurboLinkRedirectResult

The `TurboLinkRedirectResult` class allows both **Razor Pages** and **ASP.NET MVC** to respond with a redirect that works with turbolinks. TurboLinks watches **Xhr** requests and will use the JavaScript snippets from the response to set state and clear cache.

```c#
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
```

It is essential that the `POST` call happen via XHR as seen on the `Index` razor page.

```html
<form id="form" method="post" asp-controller="Values" asp-action="Index">
    <button class="btn btn-primary">
        Submit
    </button>
</form>

@section Scripts
{
    <script type="text/javascript">
     $(function () {
         
        $("#form").submit(function(e) {
            var action = $(this).attr("action");
            var data = $(this).serialize();
            var callback = function(data) {
                console.log(data);
            };
            
            $.post(action, data, callback);                 
            e.preventDefault();
        });
      });
    </script>    
}
```

We can use the `TurboLinksRedirectResult` using extension methods.

### Razor Page Example

```c#
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
        return this.TurboLinkRedirectToPage("Privacy");
    }
}
```

### Controller Example

```c#
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
```

## Conclusion

This works and can be tweaked to support more complex return values for Turbolinks, but I found through basic testing that it is unnecessary for my use case.