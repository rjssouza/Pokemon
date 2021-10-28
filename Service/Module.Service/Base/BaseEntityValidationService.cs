using Module.Dto.Base;
using Module.Repository.Interface.Base;
using Module.Repository.Model.Base;
using Module.Service.Interface.Base;
using Module.Service.Validation.Interface.Base;

namespace Module.Service.Base
{
    public abstract class BaseEntityService<TModel, TDto, TKeyType, TRepository, TValidation> : BaseEntityService<TModel, TDto, TKeyType, TRepository>
        where TModel : BaseModel
        where TRepository : IBaseCrudRepository<TModel>
        where TValidation : IBaseCrudValidation<TModel>
        where TDto : BaseDto
    {
        public abstract TValidation CrudValidation { get; set; }

        /// <summary>
        /// Efetua a chamada para validação de deleção de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public override void ValidateDeletion(TModel model)
        {
            this.CrudValidation.ValidateDeletion(model);
        }

        /// <summary>
        /// Efetua a chamada para validação de atualização de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public override void ValidateUpdate(TModel model)
        {
            this.CrudValidation.ValidateUpdate(model);
        }

        /// <summary>
        /// Efetua a chamada para validação de inserção de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public override void ValidateInsert(TModel model)
        {
            this.CrudValidation.ValidateInsert(model);
        }
    }
}