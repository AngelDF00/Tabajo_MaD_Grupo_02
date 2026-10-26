using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Es.Udc.DotNet.PracticaMAD.Model.Entities
{
    [Table("Reporte", Schema = "dbo")]
    public class Reporte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReporteId { get; set; }

        [Required]
        public long ComentarioId { get; set; }

        [Required]
        public long AutorId { get; set; }

        [Required, MaxLength(1000)]
        public string Motivo { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("ComentarioId")]
        public virtual Comentario Comentario { get; set; }

        [ForeignKey("AutorId")]
        public virtual Usuario Autor { get; set; }
    }
}