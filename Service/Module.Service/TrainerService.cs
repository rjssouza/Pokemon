using Module.Repository.Interface;
using Module.Service.Base;
using Module.Service.Interface;

namespace Module.Service
{
    public class TrainerService : BaseService, ITrainerService
    {
        public IPokemonCaptureRepository PokemonCaptureRepository { get; set; }
    }
}