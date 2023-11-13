using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace RizSoft.SiteThatRedirects
{
    public class MyRedirectRule : IRule
    {
        private readonly List<UrlRedir> _urls;
        public MyRedirectRule(List<UrlRedir> urls)
        {
                _urls = urls;
        }
        public int StatusCode { get; } = (int)HttpStatusCode.MovedPermanently;
        public bool ExcludeLocalhost { get; set; } = false;

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var host = request.Host;

          
            //IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

            //if (!memoryCache.TryGetValue("UrlList", out List<UrlRedir> urls))
            //{
                
            //    if (File.Exists("UrlList.json"))
            //    {
            //        using FileStream openStream = File.OpenRead(filename);
            //        urls = JsonSerializer.Deserialize<List<UrlRedir>>(openStream);
            //    }
            //    else
            //    {
            //        urls = null;
            //    }

            //    var cacheOptions = new MemoryCacheEntryOptions()
            //        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            //    memoryCache.Set("UrlList", urls, cacheOptions);
            //}

            if (_urls != null && _urls.Count > 0)
            {
                    var url = _urls.Find(x => x.Path.ToLower() == request.Path.ToString().ToLower());
                    if (url == null)
                    {
                        //fallback sul primo nodo
                        url = _urls.First();
                    }

                    var response = context.HttpContext.Response;
                    response.StatusCode = StatusCode;
                    response.Headers[HeaderNames.Location] = url.NewUrl;
                    context.Result = RuleResult.EndResponse; // Do not continue processing the request

                
            }
        }
    }
}
