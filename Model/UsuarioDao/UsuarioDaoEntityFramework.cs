using System.Linq;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao
{
    public class UsuarioDaoEntityFramework :
        GenericDaoEntityFramework<Usuario, long>, IUsuarioDao
    {
        public Usuario FindByLogin(string login)
        {
            return Context.Set<Usuario>()
                .SingleOrDefault(usuario => usuario.Login == login);
        }
    }
}