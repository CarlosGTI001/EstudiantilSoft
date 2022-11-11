using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EstudiantilSoft.Data;
using System;
using System.Linq;
using EstudiantilSoft.Models;

namespace EstudiantilSoft.SeedData
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EstudiantilSoftContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<EstudiantilSoftContext>>()))
            {
                // Look for any movies.
                if (context.cursos.Any())
                {
                    return;
                }

                context.cursos.AddRange(
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

                if (context.login.Any())
                {
                    return;   // DB has been seeded
                }

                context.login.AddRange(
                    new login
                    {
                        UserName = "admin",
                        UserPass = "Admin@Estudiantil01"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}