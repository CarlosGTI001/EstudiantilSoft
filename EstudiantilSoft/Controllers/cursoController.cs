using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EstudiantilSoft.Data;
using EstudiantilSoft.Models;

namespace EstudiantilSoft.Controllers
{
    public class cursoController : Controller
    {
        private readonly EstudiantilSoftContext _context;

        public cursoController(EstudiantilSoftContext context)
        {
            _context = context;
        }

        // GET: curso
        public async Task<IActionResult> Index()
        {
              return View(await _context.cursos.ToListAsync());
        }

        // GET: curso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.cursos
                .FirstOrDefaultAsync(m => m.cursoID == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: curso/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: curso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("cursoID,cursoNombre,Nivel,FechaRegistro")] curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // GET: curso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // POST: curso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("cursoID,cursoNombre,Nivel,FechaRegistro")] curso curso)
        {
            if (id != curso.cursoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cursoExists(curso.cursoID))
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
            return View(curso);
        }

        // GET: curso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.cursos
                .FirstOrDefaultAsync(m => m.cursoID == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cursos == null)
            {
                return Problem("Entity set 'EstudiantilSoftContext.cursos'  is null.");
            }
            var curso = await _context.cursos.FindAsync(id);
            if (curso != null)
            {
                _context.cursos.Remove(curso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cursoExists(int id)
        {
          return _context.cursos.Any(e => e.cursoID == id);
        }
    }
}
