using System.ComponentModel.DataAnnotations;

namespace EstudiantilSoft.Models
{
    public class Asignatura
    {
        [Key]
        public int AsignaturaID { get; set; }
        [Required]
        public string asignaturaNombre { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        
    }
}
