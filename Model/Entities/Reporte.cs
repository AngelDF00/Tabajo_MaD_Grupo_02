using System.ComponentModel.DataAnnotations.Schema;

namespace Es.Udc.DotNet.PracticaMAD.Model.Entities
{
    public partial class Reporte
    {
        // Alias de la relación AutorId para conservar el nombre usado por el módulo de reportes.
        [NotMapped]
        public virtual Usuario Autor
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
    }
}
