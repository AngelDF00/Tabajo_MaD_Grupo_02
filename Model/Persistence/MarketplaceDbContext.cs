using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

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
    }
}
