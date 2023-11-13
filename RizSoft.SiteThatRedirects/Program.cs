using Microsoft.AspNetCore.Rewrite;
using RizSoft.SiteThatRedirects;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//builder.Services.AddMemoryCache();


app.UseRewriter(new RewriteOptions().Add(new MyRedirectRule(GetUrls())));

app.UseStaticFiles();


//vince comunque la rewriteRule
app.MapGet("/", () => "Home of Redirect Site"); //this works ony if you don't have a record with path = "/"

//only for debug purpose
app.MapGet("/debug", (HttpRequest request) =>
{
    return $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
}
);

app.Run();


static List<UrlRedir> GetUrls()
{
    if (File.Exists("UrlList.json"))
    {
        using FileStream openStream = File.OpenRead("UrlList.json");
        return JsonSerializer.Deserialize<List<UrlRedir>>(openStream);
    }
    else
    {
        return null;
    }
}
