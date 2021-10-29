using Module.Dto.Base;
using System;

namespace Module.Service.Interface.Base
{
    public interface IBaseEntityService<TDto, TKeyType> : IBaseReadEntityService<TDto, TKeyType>
        where TDto : BaseDto
    {
        /// <summary>
        /// Insere os dados da entidade efetuando as validaões necessarias
        /// </summary>
        /// <param name="dtoObject">Objeto dto </param>
        TKeyType Insert(TDto dtoObject);

        /// <summary>
        /// Atualiza as informações da entidade efetuando as validações necessarias
        /// </summary>
        /// <param name="dtoObject">Objeto dto </param>
        void Update(TDto dtoObject);

        /// <summary>
        /// Remove o entidade efetuando as validações necessarias
        /// </summary>
        /// <param name="id">Identificador do anuncio de locação de imoveis</param>
        void Delete(TKeyType id);
    }
}