using Module.Factory.Interface.Conexao;
using Module.Repository.Base;
using Module.Repository.Interface;
using Module.Repository.Model;

namespace Module.Repository
{
    public class TrainerRepository : BaseCrudRepository<PokemonTrainer>, ITrainerRepository
    {
        public TrainerRepository(IDbConnectionFactory dbService)
            : base(dbService)
        {
        }
    }
}