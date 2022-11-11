using System.ComponentModel.DataAnnotations;

namespace EstudiantilSoft.Models
{
    public class Estudiantes
    {
        [Key]
        public int EstudianteID { get; set; }
        [Required]
        public string estudianteNombre { get; set; }

        [Required]
        public string estudianteApellido { get; set; }

        [Required]
        public string numeroTelefono { get; set; }

        public int SeccionID { get; set; }
        public string seccionNombre { get; set; }

        [Required]
        public string estudianteDireccion { get; set; }

        public byte[] estudianteFoto { get; set; }

        [Required]

        public DateTime FechaRegistro { get; set; }
    }

    public class EstudiantesSinImg
    {
        [Key]
        public int EstudianteID { get; set; }
        [Required]
        public string estudianteNombre { get; set; }

        [Required]
        public string estudianteApellido { get; set; }

        [Required]
        public string numeroTelefono { get; set; }

        public int SeccionID { get; set; }
        public string seccionNombre { get; set; }

        [Required]
        public string estudianteDireccion { get; set; }

        [Required]

        public DateTime FechaRegistro { get; set; }

        public Decimal Deuda { get; set; }
    }

    public class EstudianteImagen
    {
        [Key]
        public int EstudianteID { get; set; }
        [Required]
        public string estudianteNombre { get; set; }

        [Required]
        public string estudianteApellido { get; set; }

        [Required]
        public string numeroTelefono { get; set; }

        public int SeccionID { get; set; }
        public string seccionNombre { get; set; }

        [Required]
        public string estudianteDireccion { get; set; }

        public string estudianteFoto { get; set; }

        [Required]

        public DateTime FechaRegistro { get; set; }
    }
}
