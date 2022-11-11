using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EstudiantilSoft.Data;
using EstudiantilSoft.Models;
using Microsoft.Extensions.Caching.Memory;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace EstudiantilSoft.Controllers
{
    public class LoginController : Controller
    {
        private readonly EstudiantilSoftContext _context;
        private readonly IMemoryCache _memoryCache;
        public LoginController(EstudiantilSoftContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        //ViewBag.ID = id;
        //@ViewData["ID"]
        // GET: Login

        public async Task<IActionResult> Usuarios()
        {
            ViewBag.Resultado = "Limpio";

            if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
            {
                ViewBag.Resultado = "Error";
                ViewBag.noAuth = "true";
                return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

            }
            if (_memoryCache.Get("login") == null)
            {
                ViewBag.Resultado = "Error";
                return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


            }
            else
            {
                if (_memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                    return View(await _context.login.ToListAsync());
                }
                else if (_memoryCache.Get("login").ToString() == "error")
                {
                    ViewBag.Resultado = "Errado";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                }
                else
                {
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                }
            }
        }



        public IActionResult Index()
        {
            ViewBag.Resultado = "Limpio";
            
            if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
            {
                ViewBag.noAuth = "true";

            }
            if (_memoryCache.Get("login") == null)
            {
                ViewBag.Resultado = "Limpio";

                return View();
            }
            else
            {
                if (_memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Modulos");
                }
                else if (_memoryCache.Get("login").ToString() == "error")
                {
                    ViewBag.Resultado = "Errado";
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult index(login login)
        {
            var result = _context.login.FirstOrDefaultAsync(m => m.UserName == login.UserName).Result;
            
            if (result.UserPass == login.UserPass)
            {
                _memoryCache.Set("login", "true");
                
                ViewBag.Resultado = "";
            }
            else
            {
                ViewBag.Resultado = "";
                _memoryCache.Set("login", "error");
            }
            return (ActionResult)RedirectToAction("Index", "Login");
        }

        public IActionResult LogOut()
        {
            _memoryCache.Set("login", "false");
            return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Resultado = "Limpio";
           
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.noAuth = "true";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

                }
                if (_memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Error";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


                }
                else
                {
                    if (_memoryCache.Get("login").ToString() == "true")
                    {
                        ViewBag.Resultado = "Logueado";
                        if (_memoryCache.Get("user") == "admin")
                        {
                            if (id == null || _context.login == null)
                            {
                                return NotFound();
                            }

                            var login = await _context.login
                                .FirstOrDefaultAsync(m => m.UserId == id);
                            if (login == null)
                            {
                                return NotFound();
                            }

                            return View(login);
                        }
                        else
                        {
                            ViewBag.Resultado = "Errado";
                            return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                        }
                    }
                    else if (_memoryCache.Get("login").ToString() == "error")
                    {
                        ViewBag.Resultado = "Errado";
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                    else
                    {
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                }
        }

        // GET: Login/Create
        public IActionResult Create()
        {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.noAuth = "true";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

                }
                if (_memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Error";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


                }
                else
                {
                    if (_memoryCache.Get("login").ToString() == "true")
                    {
                        ViewBag.Resultado = "Logueado";
                        return View();
                    }
                    else if (_memoryCache.Get("login").ToString() == "error")
                    {
                        ViewBag.Resultado = "Errado";
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                    else
                    {
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                }
            
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,UserPass")] login login)
        {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.noAuth = "true";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

                }
                if (_memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Error";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


                }
                else
                {
                    if (_memoryCache.Get("login").ToString() == "true")
                    {
                        ViewBag.Resultado = "Logueado";
                        
                            if (ModelState.IsValid)
                            {
                                _context.Add(login);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                            return View(login);
                        
                    }
                    else if (_memoryCache.Get("login").ToString() == "error")
                    {
                        ViewBag.Resultado = "Errado";
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                    else
                    {
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                }
            
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.noAuth = "true";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

                }
                if (_memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Error";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


                }
                else
                {
                    if (_memoryCache.Get("login").ToString() == "true")
                    {
                        ViewBag.Resultado = "Logueado";
                        

                            if (id == null || _context.login == null)
                            {
                                return NotFound();
                            }

                            var login = await _context.login.FindAsync(id);
                            if (login == null)
                            {
                                return NotFound();
                            }
                            return View(login);
                    }
                    else if (_memoryCache.Get("login").ToString() == "error")
                    {
                        ViewBag.Resultado = "Errado";
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                    else
                    {
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                }

        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserPass")] login login)
        {

            if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.noAuth = "true";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

                }
                if (_memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Error";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


                }
                else
                {
                    if (_memoryCache.Get("login").ToString() == "true")
                    {
                        ViewBag.Resultado = "Logueado";

                            if (id != login.UserId)
                            {
                                return NotFound();
                            }

                            if (ModelState.IsValid)
                            {
                                try
                                {
                                    _context.Update(login);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    if (!loginExists(login.UserId))
                                    {
                                        return NotFound();
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                                return RedirectToAction(nameof(Index));
                            }
                            return View(login);
                    }
                    else if (_memoryCache.Get("login").ToString() == "error")
                    {
                        ViewBag.Resultado = "Errado";
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                    else
                    {
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                }

        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.noAuth = "true";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

                }
                if (_memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Error";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


                }
                else
                {
                    if (_memoryCache.Get("login").ToString() == "true")
                    {
                        ViewBag.Resultado = "Logueado";

                            if (id == null || _context.login == null)
                            {
                                return NotFound();
                            }

                            var login = await _context.login
                                .FirstOrDefaultAsync(m => m.UserId == id);
                            if (login == null)
                            {
                                return NotFound();
                            }

                            return View(login);
                    }
                    else if (_memoryCache.Get("login").ToString() == "error")
                    {
                        ViewBag.Resultado = "Errado";
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                    else
                    {
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                }
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "noAuth")
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.noAuth = "true";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");

                }
                if (_memoryCache.Get("login") == null)
                {
                    ViewBag.Resultado = "Error";
                    return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");


                }
                else
                {
                    if (_memoryCache.Get("login").ToString() == "true")
                    {
                        ViewBag.Resultado = "Logueado";

                            if (_context.login == null)
                            {
                                return Problem("Entity set 'EstudiantilSoftContext.login'  is null.");
                            }
                            var login = await _context.login.FindAsync(id);
                            if (login != null)
                            {
                                _context.login.Remove(login);
                            }

                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                    }
                    else if (_memoryCache.Get("login").ToString() == "error")
                    {
                        ViewBag.Resultado = "Errado";
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                    else
                    {
                        return (ActionResult)RedirectToAction(actionName: "Index", controllerName: "Login");
                    }
                }
            }
           

        private bool loginExists(int id)
        {
          return _context.login.Any(e => e.UserId == id);
        }
    }
}
