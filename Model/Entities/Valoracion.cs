using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Es.Udc.DotNet.PracticaMAD.Model.Entities
{
    [Table("Valoracion", Schema = "dbo")]
    public class Valoracion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ValoracionId { get; set; }

        [Required]
        public long ProductoId { get; set; }

        [Required]
        public long AutorId { get; set; }

        [Required]
        public int Voto { get; set; }

        [MaxLength(1000)]
        public string Comentario { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        [ForeignKey("AutorId")]
        public virtual Usuario Autor { get; set; }
    }
}