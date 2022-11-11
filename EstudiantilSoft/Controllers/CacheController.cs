using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

public class CacheController : Controller
{
    private readonly IMemoryCache _memoryCache;

    public CacheController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public ActionResult Index()
    {
        
        string cacheKey = "login";
        string cachedMessage;

        if (!this._memoryCache.TryGetValue(cacheKey, out cachedMessage))
        {
            // Create a fake delay of 3 seconds to simulate heavy processing...
            System.Threading.Thread.Sleep(3000);

            cachedMessage = "Cache was last refreshed @ " + DateTime.Now.ToLongTimeString();
            this._memoryCache.Set(cacheKey, cachedMessage);
        }

        return Content(_memoryCache.Get("login").ToString());
    }
}