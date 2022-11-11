using System.ComponentModel.DataAnnotations;

namespace EstudiantilSoft.Models
{
    public class Maestros
    {
        [Key]
        public int MaestroID { get; set; }
        [Required]
        public string maestroNombre { get; set; }

        [Required]
        public string maestroApellido { get; set; }

        [Required]
        public string maestroCedula { get; set; }

        [Required]
        public string numeroTelefono { get; set; }

        public int AsignaturaID { get; set; }
        public string asignaturaNombre { get; set; }

        [Required]
        public string maestroDireccion { get; set; }

        public byte[] maestroFoto { get; set; }

        [Required]

        public DateTime FechaRegistro { get; set; }
    }
}
