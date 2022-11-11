using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using EstudiantilSoft.Models;
namespace EstudiantilSoft.Controllers
{
    public class Page : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public Page(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        
        // GET: Page
        public ActionResult Index()
        {
            try
            {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                }
                else
                {
                    
                    ViewBag.Resultado = "Limpio";
                }
                return View();
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
                return View();
            }
        }

        public ActionResult Login(login login)
        {
            try
            {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                }
                else
                {
                    ViewBag.Resultado = "Limpio";
                }
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
            }
            try
            {
                if(this._memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Limpio";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                }
                else
                {
                    ViewBag.Resultado = "Logued";
                    return this._memoryCache.Get("login").ToString() == "true"
                    ? RedirectToAction(actionName: "Index", controllerName: "Modulos")
                    : (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login"); 
                }
                
            }
            catch
            {
                return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
            }
        }



    }
}
