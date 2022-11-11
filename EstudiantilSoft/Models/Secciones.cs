using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace EstudiantilSoft.Models
{
    public class Secciones
    {
        [Key]
        [Required]
        public int SeccionID { get; set; }

        [Required]
        [Display(Name = "Seccion")]
        public string seccionNombre { get; set; }

        [Required]
        public int cursoID { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public string cursoNombre { get; set; }

        [Required]
        public int MaestroID { get; set; }

        [Display(Name = "Maestro")]
        public string maestroNombre { get; set; }

        public string maestroApellido { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
