using System.ComponentModel.DataAnnotations;

namespace EstudiantilSoft.Models
{
    public class curso
    {
        [Key]
        [Required]
        public int cursoID { get; set; }

        [Required]
        public string cursoNombre { get; set; }
        [Required]
        public string Nivel { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        
    }
}
