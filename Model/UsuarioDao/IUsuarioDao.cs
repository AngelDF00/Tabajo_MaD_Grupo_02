using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao
{
    public interface IUsuarioDao : IGenericDao<Usuario, long>
    {
        Usuario FindByLogin(string login);
    }
}