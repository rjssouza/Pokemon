using Module.Factory.Interface.Base;
using System.Data;

namespace Module.Factory.Interface.Conexao
{
    public interface IDbConnectionFactory : IBaseFactory
    {
        IDbConnection DbConnection { get; }

        IDbTransaction Transaction { get; }
    }
}