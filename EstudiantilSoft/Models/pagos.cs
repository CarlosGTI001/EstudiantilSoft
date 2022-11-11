using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace EstudiantilSoft.Models
{
    public class pagos
    {
        [Display(Name = "ID")]
        [Key]
        public int DeudaID { get; set; }

        [Display(Name = "Matricula")]
        public int EstudianteID { get; set; }

        [DataType(DataType.Date)]
        public DateTime DeudaFecha { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DeudaValor { get; set; }
        [Display(Name = "Concepto")]
        public string DeudaConcepto { get; set; }
    }
}
