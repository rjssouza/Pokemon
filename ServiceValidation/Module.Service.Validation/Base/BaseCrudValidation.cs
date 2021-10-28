using Module.Dto.Validation.Api;
using Module.Repository.Model.Base;
using Module.Service.Validation.Interface.Base;

namespace Module.Service.Validation.Base
{
    /// <summary>
    /// Classe com preenchimento pré estabelecido para a validação de uma determinada entidade modelo
    /// </summary>
    /// <typeparam name="TModel">Tipo da entidade herdando de model</typeparam>
    public abstract class BaseCrudValidation<TModel> : BaseValidation, IBaseCrudValidation<TModel>
            where TModel : BaseModel
    {
        public BaseCrudValidation()
        {
        }

        /// <summary>
        /// Efetua a chamada para validação de atualização de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public virtual void ValidateUpdate(TModel model)
        {
            ValidarModelo(model);
        }

        /// <summary>
        /// Efetua a chamada para validação de deleção de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public virtual void ValidateDeletion(TModel model)
        {
            ValidarModelo(model);
        }

        /// <summary>
        /// Efetua a chamada para validação de inserção de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public virtual void ValidateInsert(TModel model)
        {
            ValidarModelo(model);
        }

        /// <summary>
        /// Valida regras de negocio basicas implementadas no modelo de dados
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        private static void ValidarModelo(TModel model)
        {
            if (model == null)
                throw new ValidationException(typeof(TModel).Name, "Objeto nulo não pode ser validado");
        }
    }
}