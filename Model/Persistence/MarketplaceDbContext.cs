using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace Es.Udc.DotNet.PracticaMAD.Model.Persistence
{
    public class MarketplaceDbContext : DbContext
    {
        static MarketplaceDbContext()
        {
            Database.SetInitializer<MarketplaceDbContext>(null);
        }

        public MarketplaceDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Valoracion> Valoraciones { get; set; }
        public DbSet<Reporte> Reportes { get; set; }

    }
}
