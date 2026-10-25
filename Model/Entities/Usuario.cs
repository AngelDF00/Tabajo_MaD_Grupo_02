using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Es.Udc.DotNet.PracticaMAD.Model.Entities
{
    [Table("Usuario", Schema = "dbo")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UsuarioId { get; set; }

        [Required, MaxLength(30)]
        public string Login { get; set; }

        [Required, MaxLength(200)]
        public string HashContrasena { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        public string Apellidos { get; set; }

        [Required, MaxLength(254)]
        public string Correo { get; set; }

        [MaxLength(10)]
        public string Idioma { get; set; }

        [MaxLength(100)]
        public string Pais { get; set; }
    }
}
