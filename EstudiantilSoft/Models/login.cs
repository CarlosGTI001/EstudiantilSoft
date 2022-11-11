using System.ComponentModel.DataAnnotations;

namespace EstudiantilSoft.Models
{
    public class login
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{5,10}$",
         ErrorMessage = "Debe contener de 5 a 10 Caracteres, y solo incluir caracteres especiales como el '-'.")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
         ErrorMessage = "Debe contener mas de 8 caracteres, entre ellos numeros, letras mayusculas y un caracter alfa numerico")]
        public string UserPass { get; set; }
    }
}
