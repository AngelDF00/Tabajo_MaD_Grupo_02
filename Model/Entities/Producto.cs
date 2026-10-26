using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Es.Udc.DotNet.PracticaMAD.Model.Entities
{
    [Table("Producto", Schema = "dbo")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductoId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public long CategoriaId { get; set; }

        [Required]
        public Categoria Categoria { get; set; }

        [Required]
        public DateTime FechaAlta { get; set; }
    }
}
