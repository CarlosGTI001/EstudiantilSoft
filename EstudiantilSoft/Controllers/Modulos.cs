using EstudiantilSoft.Data;
using EstudiantilSoft.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;
using Secciones = EstudiantilSoft.Models.Secciones;

namespace EstudiantilSoft.Controllers
{
    
    public class Modulos : Controller
    {
        private IMemoryCache _memoryCache;
        private readonly EstudiantilSoftContext _context;

        public Modulos(EstudiantilSoftContext context, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

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
                    this._memoryCache.Set("login", "noAuth");
                    ViewBag.noAuth = "true";
                    return RedirectToAction(controllerName: "Login", actionName: "Index");
                }
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
            }
            return View();
        }

        public async Task <IActionResult> Cursos()
        {
            try
            {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                }
                else
                {
                    this._memoryCache.Set("login", "noAuth");
                    ViewBag.noAuth = "true";
                    return RedirectToAction(controllerName: "Login", actionName: "Index");
                }
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
            }
            var cursOs = _context.cursos.Select(a => a);
            return View(await cursOs.ToListAsync());
        }

       

        public IActionResult FiltrarCursosFechaExcel(DateTime fechaInicial, DateTime fechaFinal)
        {
                var fechaExport = fechaFinal;
                fechaFinal = fechaFinal.AddDays(1);
                var ticks1 = fechaInicial.Ticks;
                var ticks2 = fechaFinal.Ticks;
                var cursos = _context.cursos.Select(a => a);
                var filtrado = cursos
                    .AsEnumerable()
                    .Where(a =>
                        ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                      );
             

            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<curso>(filtrado.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);

                    var nombreExcel = string.Concat("Reporte Curso filtrado: "+fechaInicial.ToString("dd-MM-yyyy")+" A "+fechaExport.ToString("dd-MM-yyyy"), "Exportado el dia: ", DateTime.Now.ToString("dd-MM-yyyy"), ".xlsx");

                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        public IActionResult FiltrarCursosExcel()
        {
            var cursos = _context.cursos.Select(a => a);
            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<curso>(cursos.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);

                    var nombreExcel = string.Concat("Reporte Curso ", DateTime.Now.ToString(), ".xlsx");

                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }
       
        public IActionResult FiltrarCursos(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                fechaFinal = fechaFinal.AddDays(1);
                var ticks1 = fechaInicial.Ticks;
                var ticks2 = fechaFinal.Ticks;
                var cursos = _context.cursos.Select(a => a);
                var json = JsonConvert.SerializeObject(cursos
                        .AsEnumerable()
                        .Where(a => 
                            ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                          )
                     );
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult FiltrarCursosID(int cursoID)
        {
            try
            {
                var cursos = _context.cursos.Find(cursoID);
                var json = JsonConvert.SerializeObject(cursos);
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        //[HttpPost]
        public IActionResult ModificarCurso(int cursoID, string nombreCurso, string Nivel){
            var cursoEditar = _context.cursos.Find(cursoID);
            cursoEditar.cursoNombre = nombreCurso;
            cursoEditar.Nivel = Nivel;
            _context.Update(cursoEditar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }



        public IActionResult InsertarCurso(string cursoNombre, string Nivel)
        {

            _context.cursos.Add(new curso
                {
                    cursoNombre = cursoNombre,
                    Nivel = Nivel,
                    FechaRegistro = DateTime.Now
                }
            );
            _context.SaveChanges();
            return Json("{Ok:'ok'}");
        }
        public IActionResult EliminarCurso(int cursoID)
        {
            var cursoEliminar = _context.cursos.Find(cursoID);
            _context.Remove(cursoEliminar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }

        

        public IActionResult LeerCursos()
        {
            try
            {
                if(_context.cursos.Select(a=>a).Count() == 0)
                {
                    _context.cursos.AddRange(
                        new curso
                        {
                            cursoNombre = "1ro",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        },
                        new curso
                        {
                            cursoNombre = "2do",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        },
                        new curso
                        {
                            cursoNombre = "3ro",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        },
                        new curso
                        {
                            cursoNombre = "4to",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        },
                        new curso
                        {
                            cursoNombre = "5to",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        },
                        new curso
                        {
                            cursoNombre = "6to",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        },
                        new curso
                        {
                            cursoNombre = "7mo",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        },
                        new curso
                        {
                            cursoNombre = "8vo",
                            FechaRegistro = DateTime.Now,
                            Nivel = "basico"
                        }, new curso
                        {
                            cursoNombre = "1ro",
                            FechaRegistro = DateTime.Now,
                            Nivel = "medio"
                        },
                        new curso
                        {
                            cursoNombre = "2do",
                            FechaRegistro = DateTime.Now,
                            Nivel = "medio"
                        },
                        new curso
                        {
                            cursoNombre = "3ro",
                            FechaRegistro = DateTime.Now,
                            Nivel = "medio"
                        },
                        new curso
                        {
                            cursoNombre = "4to",
                            FechaRegistro = DateTime.Now,
                            Nivel = "medio"
                        }
                    );
                    _context.SaveChanges();
                }
                var json = JsonConvert.SerializeObject(_context.cursos.Select(a=>a));
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Asignaturas()
        {
            try
            {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                }
                else
                {
                    this._memoryCache.Set("login", "noAuth");
                    ViewBag.noAuth = "true";
                    return RedirectToAction(controllerName: "Login", actionName: "Index");
                }
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
            }
            var maestros = _context.asignatura.Select(a => a);
            return View(await maestros.ToListAsync());
        }

        public IActionResult FiltrarAsignaturasFechaExcel(DateTime fechaInicial, DateTime fechaFinal)
        {
            var fechaExport = fechaFinal;
            fechaFinal = fechaFinal.AddDays(1);
            var ticks1 = fechaInicial.Ticks;
            var ticks2 = fechaFinal.Ticks;
            var maestros = _context.asignatura.Select(a => a);
            var filtrado = maestros
                .AsEnumerable()
                .Where(a =>
                    ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                  );


            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<Asignatura>(filtrado.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);

                    var nombreExcel = string.Concat("Reporte asignatura filtrado: " + fechaInicial.ToString("dd-MM-yyyy") + " A " + fechaExport.ToString("dd-MM-yyyy"), "Exportado el dia: ", DateTime.Now.ToString("dd-MM-yyyy"), ".xlsx");

                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        public IActionResult FiltrarAsignaturasExcel()
        {
            var maestros = _context.asignatura.Select(a => a);
            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<Asignatura>(maestros.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);

                    var nombreExcel = string.Concat("Reporte asignatura ", DateTime.Now.ToString(), ".xlsx");

                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        public IActionResult filtrarAsignaturas(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                fechaFinal = fechaFinal.AddDays(1);
                var ticks1 = fechaInicial.Ticks;
                var ticks2 = fechaFinal.Ticks;
                var maestros = _context.asignatura.Select(a => a);
                var json = JsonConvert.SerializeObject(maestros
                        .AsEnumerable()
                        .Where(a =>
                            ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                          )
                     );
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult FiltrarAsignaturaID(int asignaturaID)
        {
            try
            {
                var maestros = _context.asignatura.Find(asignaturaID);
                var json = JsonConvert.SerializeObject(maestros);
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        //[HttpPost]
        public IActionResult ModificarAsignatura(int asignaturaID, string nombreasignatura, string Nivel)
        {
            var asignaturaEditar = _context.asignatura.Find(asignaturaID);
            asignaturaEditar.asignaturaNombre = nombreasignatura;
            _context.Update(asignaturaEditar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }



        public IActionResult InsertarAsignatura(string asignaturaNombre, string Nivel)
        {

            _context.asignatura.Add(new Asignatura
            {
                asignaturaNombre = asignaturaNombre,
                FechaRegistro = DateTime.Now
            }
            );
            _context.SaveChanges();
            return Json("{Ok:'ok'}");
        }
        public IActionResult EliminarAsignaturas(int asignaturaID)
        {
            var asignaturaEliminar = _context.asignatura.Find(asignaturaID);
            _context.Remove(asignaturaEliminar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }



        public IActionResult LeerAsignaturas()
        {
            try
            {
                if (_context.asignatura.Select(a => a).Count() == 0)
                {
                    _context.asignatura.AddRange(
                        new Asignatura
                        {
                            asignaturaNombre = "Español",
                            FechaRegistro = DateTime.Now
                        },
                        new Asignatura
                        {
                            asignaturaNombre = "Naturales",
                            FechaRegistro = DateTime.Now
                            
                        },
                        new Asignatura
                        {
                            asignaturaNombre = "Sociales",
                            FechaRegistro = DateTime.Now
                        }
                    );
                    _context.SaveChanges();
                }
                var json = JsonConvert.SerializeObject(_context.asignatura.Select(a => a));
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


        public async Task<IActionResult> Maestros()
        {
            try
            {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                }
                else
                {
                    this._memoryCache.Set("login", "noAuth");
                    ViewBag.noAuth = "true";
                    return RedirectToAction(controllerName: "Login", actionName: "Index");
                }
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
            }
            var Maestros = _context.maestros.Select(a => a);
            return View(await Maestros.ToListAsync());
        }

        public IActionResult FiltrarMaestrosFechaExcel(DateTime fechaInicial, DateTime fechaFinal)
        {
            var fechaExport = fechaFinal;
            fechaFinal = fechaFinal.AddDays(1);
            var ticks1 = fechaInicial.Ticks;
            var ticks2 = fechaFinal.Ticks;
            var Maestros = _context.maestros.Select(a => a);
            var filtrado = Maestros
                .AsEnumerable()
                .Where(a =>
                    ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                  );
            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<Maestros>(filtrado.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("Reporte maestros filtrado: " + fechaInicial.ToString("dd-MM-yyyy") + " A " + fechaExport.ToString("dd-MM-yyyy"), "Exportado el dia: ", DateTime.Now.ToString("dd-MM-yyyy"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }



        public IActionResult FiltrarMaestrosExcel()
        {
            var MaestrosSinImg = _context.maestros.Select(a=>a);
            var mmImg = new List<maestroSinImg>();
            foreach(var x in MaestrosSinImg)
            {
                mmImg.Add(new maestroSinImg
                {
                    maestroApellido = x.maestroApellido,
                    maestroNombre = x.maestroNombre,
                    maestroCedula = x.maestroCedula,
                    maestroDireccion = x.maestroDireccion,
                    MaestroID = x.MaestroID,
                    FechaRegistro = x.FechaRegistro,
                    AsignaturaID = x.AsignaturaID,
                    asignaturaNombre = x.asignaturaNombre,
                    numeroTelefono = x.numeroTelefono
                });
            }
            
            using (var libro = new XLWorkbook())
            {
               
                var hoja = libro.Worksheets.Add(ToDataTable<maestroSinImg>(mmImg));
                hoja.ColumnsUsed().AdjustToContents();
                hoja.Name = "Maestros";
                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("Reporte maestros ", DateTime.Now.ToString(), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        public IActionResult FiltrarMaestros(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                fechaFinal = fechaFinal.AddDays(1);
                var ticks1 = fechaInicial.Ticks;
                var ticks2 = fechaFinal.Ticks;
                var Maestros = _context.maestros.Select(a => a);
                var json = JsonConvert.SerializeObject(Maestros
                        .AsEnumerable()
                        .Where(a =>
                            ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                          )
                     );
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult FiltrarMaestrosID(int maestrosID)
        {
            try
            {
                var Maestros = _context.maestros.Find(maestrosID);
                var json = JsonConvert.SerializeObject(Maestros);
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult ModificarMaestros(int maestrosID, string maestroNombre, string maestroApellido, string maestroCedula, string numeroTelefono, string maestroDireccion, string maestroFoto, int AsignaturaID)
        {
            var maestrosEditar = _context.maestros.Find(maestrosID);
            maestrosEditar.maestroNombre = maestroNombre;
            maestrosEditar.maestroApellido = maestroApellido;
            maestrosEditar.maestroCedula = maestroCedula;
            maestrosEditar.numeroTelefono = numeroTelefono;
            maestrosEditar.maestroFoto = Convert.FromBase64String(maestroFoto);
            maestrosEditar.maestroDireccion = maestroDireccion;
            maestrosEditar.AsignaturaID = AsignaturaID;
            maestrosEditar.asignaturaNombre = _context.asignatura.Where(a => a.AsignaturaID == AsignaturaID).Select(a => a.asignaturaNombre).FirstOrDefault();
            _context.Update(maestrosEditar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }


        
        public IActionResult Eliminarmaestros(int maestrosID)
        {
            var maestrosEliminar = _context.maestros.Find(maestrosID);
            _context.Remove(maestrosEliminar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }



        public IActionResult LeerMaestros()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_context.maestros.Select(a => a));
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }




        public DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public async Task<IActionResult> Secciones()
        {
            try
            {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                }
                else
                {
                    this._memoryCache.Set("login", "noAuth");
                    ViewBag.noAuth = "true";
                    return RedirectToAction(controllerName: "Login", actionName: "Index");
                }
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
            }
            var maestros = _context.secciones.Select(a => a);
            return View(await maestros.ToListAsync());
        }

        public IActionResult FiltrarSeccionesFechaExcel(DateTime fechaInicial, DateTime fechaFinal)
        {
            var fechaExport = fechaFinal;
            fechaFinal = fechaFinal.AddDays(1);
            var ticks1 = fechaInicial.Ticks;
            var ticks2 = fechaFinal.Ticks;
            var secciones = _context.secciones.Select(a => a);
            var filtrado = secciones
                .AsEnumerable()
                .Where(a =>
                    ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                  );


            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<Secciones>(filtrado.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("Reporte seccion filtrado: " + fechaInicial.ToString("dd-MM-yyyy") + " A " + fechaExport.ToString("dd-MM-yyyy"), "Exportado el dia: ", DateTime.Now.ToString("dd-MM-yyyy"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        public IActionResult FiltrarSeccionesExcel()
        {
            var maestros = _context.secciones.Select(a => a);
            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<Secciones>(maestros.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);

                    var nombreExcel = string.Concat("Reporte seccion ", DateTime.Now.ToString(), ".xlsx");

                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        public IActionResult filtrarSecciones(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                fechaFinal = fechaFinal.AddDays(1);
                var ticks1 = fechaInicial.Ticks;
                var ticks2 = fechaFinal.Ticks;
                var secciones = _context.secciones.Select(a => a);
                var filtrado = secciones
                    .AsEnumerable()
                    .Where(a =>
                        ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                );
                var retorno = JsonConvert.SerializeObject(filtrado);
                return Json(retorno);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult filtrarSeccionesID(int seccionID)
        {
            try
            {
                var maestros = _context.secciones.Find(seccionID);
                var json = JsonConvert.SerializeObject(maestros);
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        //[HttpPost]
        public IActionResult ModificarSeccion(int seccionID, string seccionNombre, int maestroID, int cursoID)
        {
            try
            {
                string name = _context.cursos.Find(cursoID).cursoNombre;
                string nameSinNivel = char.ToString(name[0]) + char.ToString(name[1]) + char.ToString(name[2]);
                var nivel = _context.cursos.Find(cursoID).Nivel;
                var seccionEditar = _context.secciones.Find(seccionID);
                seccionEditar.seccionNombre = nameSinNivel + seccionNombre + " de " + nivel;
                seccionEditar.cursoID = cursoID;
                seccionEditar.cursoNombre = _context.cursos.Find(cursoID).cursoNombre;
                seccionEditar.MaestroID = maestroID;
                seccionEditar.maestroNombre = _context.maestros.Find(maestroID).maestroNombre;
                seccionEditar.maestroApellido = _context.maestros.Find(maestroID).maestroApellido;
                _context.Update(seccionEditar);
                _context.SaveChanges();
                return Json("{'guardado'='true'}");
            }
            catch
            {
                return BadRequest();
            }
            
        }



        public IActionResult InsertarSeccion(string seccionNombre, int maestroID, int cursoID)
        {
            string name = _context.cursos.Find(cursoID).cursoNombre;
            string nameSinNivel = char.ToString(name[0]) + char.ToString(name[1]) + char.ToString(name[2]);
            var nivel = _context.cursos.Find(cursoID).Nivel;
            _context.secciones.Add(new Secciones
                {
                    seccionNombre = nameSinNivel + seccionNombre + " de " + nivel,
                    cursoID = cursoID,
                    cursoNombre = _context.cursos.Find(cursoID).cursoNombre,
                    MaestroID = maestroID,
                    maestroNombre = _context.maestros.Find(maestroID).maestroNombre,
                    maestroApellido = _context.maestros.Find(maestroID).maestroApellido,
                    FechaRegistro = DateTime.Now
                }       
            );
            _context.SaveChanges();
            return Json("{Ok:'ok'}");
        }
        public IActionResult EliminarSecciones(int seccionID)
        {
            try
            {
                var seccionEliminar = _context.secciones.Find(seccionID);
                _context.Remove(seccionEliminar);
                _context.SaveChanges();
                return Json("{'guardado'='true'}");
            }
            catch
            {
                return BadRequest();
            }
        }



        public IActionResult LeerSecciones()
        {
            try
            {
                if (_context.secciones.Select(a => a).Count() == 0)
                {
                    _context.secciones.AddRange(
                    );
                    _context.SaveChanges();
                }
                var json = JsonConvert.SerializeObject(_context.secciones.Select(a => a));
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Estudiantes()
        {
            try
            {
                if (this._memoryCache.Get("login") != null && this._memoryCache.Get("login").ToString() == "true")
                {
                    ViewBag.Resultado = "Logueado";
                }
                else
                {
                    this._memoryCache.Set("login", "noAuth");
                    ViewBag.noAuth = "true";
                    return RedirectToAction(controllerName: "Login", actionName: "Index");
                }
            }
            catch
            {
                ViewBag.Resultado = "Limpio";
            }
            var Estudiantes = _context.Estudiantes.Select(a => a);
            return View(await Estudiantes.ToListAsync());
        }

        public IActionResult FiltrarEstudiantesFechaExcel(DateTime fechaInicial, DateTime fechaFinal)
        {
            var fechaExport = fechaFinal;
            fechaFinal = fechaFinal.AddDays(1);
            var ticks1 = fechaInicial.Ticks;
            var ticks2 = fechaFinal.Ticks;
            var Estudiantes = _context.Estudiantes.Select(a => a);
            var filtrado = Estudiantes
                .AsEnumerable()
                .Where(a =>
                    ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                  );
           
            var mmImg = new List<EstudiantesSinImg>();
            foreach (var x in filtrado)
            {
                var deuda = _context.pagos.Where(a => a.EstudianteID == x.EstudianteID).Select(a => a.DeudaValor).Sum();
                mmImg.Add(new EstudiantesSinImg
                {
                    estudianteApellido = x.estudianteApellido,
                    estudianteNombre = x.estudianteNombre,
                    estudianteDireccion = x.estudianteDireccion,
                    EstudianteID = x.EstudianteID,
                    FechaRegistro = x.FechaRegistro,
                    SeccionID = x.SeccionID,
                    seccionNombre = x.seccionNombre,
                    numeroTelefono = x.numeroTelefono,
                    Deuda = deuda
                });
            }
            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(ToDataTable<EstudiantesSinImg>(mmImg.ToList()));
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("Reporte estudiantes filtrado: " + fechaInicial.ToString("dd-MM-yyyy") + " A " + fechaExport.ToString("dd-MM-yyyy"), "Exportado el dia: ", DateTime.Now.ToString("dd-MM-yyyy"), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }



        public IActionResult FiltrarEstudiantesExcel()
        {
            var EstudiantesSinImg = _context.Estudiantes.Select(a => a);
            var mmImg = new List<EstudiantesSinImg>();
            foreach (var x in EstudiantesSinImg)
            {
                var deuda = _context.pagos.Where(a => a.EstudianteID == x.EstudianteID).Select(a => a.DeudaValor).Sum();
                mmImg.Add(new EstudiantesSinImg
                {
                    estudianteApellido = x.estudianteApellido,
                    estudianteNombre = x.estudianteNombre,
                    estudianteDireccion = x.estudianteDireccion,
                    EstudianteID = x.EstudianteID,
                    FechaRegistro = x.FechaRegistro,
                    SeccionID = x.SeccionID,
                    seccionNombre = x.seccionNombre,
                    numeroTelefono = x.numeroTelefono,
                    Deuda = deuda
                });
            }

            using (var libro = new XLWorkbook())
            {

                var hoja = libro.Worksheets.Add(ToDataTable<EstudiantesSinImg>(mmImg));
                hoja.ColumnsUsed().AdjustToContents();
                hoja.Name = "Estudiantes";
                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("Reporte estudiantes ", DateTime.Now.ToString(), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }

        public IActionResult FiltrarEstudiantes(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                fechaFinal = fechaFinal.AddDays(1);
                var ticks1 = fechaInicial.Ticks;
                var ticks2 = fechaFinal.Ticks;
                var Estudiantes = _context.Estudiantes.Select(a => a);
                var json = JsonConvert.SerializeObject(Estudiantes
                        .AsEnumerable()
                        .Where(a =>
                            ((a.FechaRegistro.Ticks >= ticks1) && (a.FechaRegistro.Ticks <= ticks2))
                          )
                     );
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult FiltrarEstudiantesID(int EstudianteID)
        {
            try
            {
                var Estudiantes = _context.Estudiantes.Find(EstudianteID);
                var json = JsonConvert.SerializeObject(Estudiantes);
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult ModificarEstudiantes(int EstudianteID, string estudianteNombre, string estudianteApellido, string estudianteCedula, string numeroTelefono, string estudianteDireccion, string estudianteFoto, int SeccionID)
        {
            var estudiantesEditar = _context.Estudiantes.Find(EstudianteID);
            estudiantesEditar.estudianteNombre = estudianteNombre;
            estudiantesEditar.estudianteApellido = estudianteApellido;
            estudiantesEditar.numeroTelefono = numeroTelefono;
            estudiantesEditar.estudianteFoto = Convert.FromBase64String(estudianteFoto);
            estudiantesEditar.estudianteDireccion = estudianteDireccion;
            estudiantesEditar.SeccionID = SeccionID;
            estudiantesEditar.seccionNombre = _context.secciones.Where(a => a.SeccionID == SeccionID).Select(a => a.seccionNombre).FirstOrDefault();
            _context.Update(estudiantesEditar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }



        public IActionResult Eliminarestudiantes(int EstudianteID)
        {
            var estudiantesEliminar = _context.Estudiantes.Find(EstudianteID);
            _context.Remove(estudiantesEliminar);
            _context.SaveChanges();
            return Json("{'guardado'='true'}");
        }

        public IActionResult LeerEstudiantes()
        {
            try
            {
                if (_context.Estudiantes.Select(a => a).Count() == 0)
                {
                    _context.Estudiantes.AddRange(
                    );
                    _context.SaveChanges();
                }
                var json = JsonConvert.SerializeObject(_context.Estudiantes.Select(a => a));
                return Json(json);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CrearDeuda([FromBody] DeudaCrear deudaCrear)
        {
            try
            {
                _context.pagos.Add(new pagos
                {
                    EstudianteID = deudaCrear.EstudianteID,
                    DeudaValor = deudaCrear.DeudaValor,
                    DeudaConcepto = deudaCrear.DeudaConcepto,
                    DeudaFecha = DateTime.Now
                });
                _context.SaveChanges();
                return Json(deudaCrear);
            }
            catch
            {
                return BadRequest(deudaCrear);
            }
        }

        
    }

    public class DeudaCrear
    {
        public int EstudianteID { get; set; }
        public string DeudaConcepto { get; set; }
        public decimal DeudaValor { get; set; }
    }
}
