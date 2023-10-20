using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace RizSoft.SiteThatRedirects
{
    public class MyRedirectRule : IRule
    {
        public int StatusCode { get; } = (int)HttpStatusCode.MovedPermanently;
        public bool ExcludeLocalhost { get; set; } = false;

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var host = request.Host;

            string filename = "UrlList.json";
            if (File.Exists(filename))
            {       
                using FileStream openStream = File.OpenRead(filename);
                var urls = JsonSerializer.Deserialize<List<UrlRedir>>(openStream);  

                //TODO cache after 1st load
                // how to inject Imemorycache??
                //if (!_memoryCache.TryGetValue("myKey", out List<UrlRedir> urls))
                //{
                //    urls = JsonSerializer.Deserialize<List<UrlRedir>>(openStream);
                //    _memoryCache.Set("myKey", urls, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                //}

                if (urls != null)
                {
                    var url = urls.Find(x => x.Path == request.Path.ToString().ToLower());
                    if (url != null)
                    {
                        var response = context.HttpContext.Response;
                        response.StatusCode = StatusCode;
                        response.Headers[HeaderNames.Location] = url.NewUrl;
                        context.Result = RuleResult.EndResponse; // Do not continue processing the request
                    }
                }
            }
        }
    }
}
