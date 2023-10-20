using Microsoft.AspNetCore.Rewrite;
using RizSoft.SiteThatRedirects;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//builder.Services.AddMemoryCache();

app.UseRewriter(new RewriteOptions().Add(new MyRedirectRule()));

app.UseStaticFiles();

app.MapGet("/", () => "Home of Redirect Site"); //this works ony if you don't have a record with path = "/"

//only for debug purpose
app.MapGet("/give-me-request", (HttpRequest request) =>
{
    return $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
}
);

app.Run();
