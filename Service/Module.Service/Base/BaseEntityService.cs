using Module.Dto.Base;
using Module.Repository.Interface.Base;
using Module.Repository.Model.Base;
using Module.Service.Interface.Base;
using System;

namespace Module.Service.Base
{
    public abstract class BaseEntityService<TModel, TDto, TKeyType, TRepository> : BaseService, IBaseEntityService<TDto, TKeyType>
        where TModel : BaseModel
        where TRepository : IBaseCrudRepository<TModel>
        where TDto : BaseDto
    {
        public abstract TRepository CrudRepository { get; set; }

        /// <summary>
        /// Remove o entidade efetuando as validações necessarias
        /// </summary>
        /// <param name="id">Identificador do anuncio de locação de imoveis</param>
        public virtual void Delete(TKeyType id)
        {
            var model = this.CrudRepository.GetEntityById(id);
            this.ValidateDeletion(model);

            this.OpenTransaction();
            this.CrudRepository.Delete(model);

            this.Commit();
        }

        /// <summary>
        /// Obtem pelo identificador
        /// </summary>
        /// <param name="id">Identificador da entidade</param>
        /// <returns>Objeto de dados da entidade</returns>
        public virtual TDto GetById(TKeyType id)
        {
            var model = this.CrudRepository.GetEntityById<TKeyType>(id);
            var resultado = this.ObjectConverterFactory.ConvertTo<TDto>(model);

            return resultado;
        }

        /// <summary>
        /// Insere os dados da entidade efetuando as validaões necessarias
        /// </summary>
        /// <param name="dtoObject">Objeto dto </param>
        public virtual Guid Insert(TDto dtoObject)
        {
            dtoObject.Id = Guid.NewGuid();
            var model = this.ObjectConverterFactory.ConvertTo<TModel>(dtoObject);
            this.ValidateInsert(model);

            this.OpenTransaction();
            var result = this.CrudRepository.Insert(model);

            this.Commit();

            return result;
        }

        /// <summary>
        /// Atualiza as informações da entidade efetuando as validações necessarias
        /// </summary>
        /// <param name="dtoObject">Objeto dto </param>
        public virtual void Update(TDto dtoObject)
        {
            var model = this.ObjectConverterFactory.ConvertTo<TModel>(dtoObject);
            this.ValidateUpdate(model);

            this.OpenTransaction();
            this.CrudRepository.Update(model);

            this.Commit();
        }

        /// <summary>
        /// Efetua a chamada para validação de deleção de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public virtual void ValidateUpdate(TModel model)
        {
        }

        /// <summary>
        /// Efetua a chamada para validação de atualização de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public virtual void ValidateDeletion(TModel model)
        {
        }

        /// <summary>
        /// Efetua a chamada para validação de inserção de dados da entidade
        /// </summary>
        /// <param name="model">Entidade de modelo </param>
        public virtual void ValidateInsert(TModel model)
        {
        }
    }
}