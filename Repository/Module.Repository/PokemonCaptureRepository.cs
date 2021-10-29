using Module.Factory.Interface.Conexao;
using Module.Repository.Base;
using Module.Repository.Interface;
using Module.Repository.Model;

namespace Module.Repository
{
    public class PokemonCaptureRepository : BaseCrudRepository<PokemonCapture>, IPokemonCaptureRepository
    {
        public PokemonCaptureRepository(IDbConnectionFactory dbService) : base(dbService)
        {
        }
    }
}