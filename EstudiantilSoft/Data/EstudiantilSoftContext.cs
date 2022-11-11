using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EstudiantilSoft.Models;

namespace EstudiantilSoft.Data
{
    public class EstudiantilSoftContext : DbContext
    {
        public EstudiantilSoftContext(DbContextOptions<EstudiantilSoftContext> options)
            : base(options)
        {
        }

        public DbSet<login> login { get; set; } = default!;
        public DbSet<curso> cursos { get; set; } = default!;
        public DbSet<Asignatura> asignatura { get; set; } = default!;
        public DbSet<Maestros> maestros { get; set; } = default!;

        public DbSet<Secciones> secciones { get; set; } = default!;

        public DbSet<Estudiantes> Estudiantes { get; set; } = default!;

        public DbSet<pagos> pagos { get; set; } = default!;
    }
        
}
