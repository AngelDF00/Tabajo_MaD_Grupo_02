using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Es.Udc.DotNet.PracticaMAD.Model.Entities
{
    
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CategoriaId { get; set; }

        [Required]
        public string Nombre { get; set; }
    }
}
