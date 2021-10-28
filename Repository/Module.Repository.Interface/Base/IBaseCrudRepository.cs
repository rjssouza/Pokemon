using Module.Repository.Model.Base;
using System;
using System.Collections.Generic;

namespace Module.Repository.Interface.Base
{
    public interface IBaseCrudRepository<TModel> : IBaseRepository
        where TModel : BaseModel
    {
        /// <summary>
        /// Atualiza um modelo em banco
        /// </summary>
        /// <param name="entity">Modelo informado</param>
        /// <returns>Resultado da ação</returns>
        int Update(TModel model);

        /// <summary>
        /// Insere entidade em banco
        /// </summary>
        /// <param name="entity">Modelo informado</param>
        /// <returns>Resultado da ação</returns>
        Guid Insert(TModel model);

        /// <summary>
        /// Obtem modelo pelo id informado
        /// </summary>
        /// <param name="id">Identificador do modelo</param>
        /// <returns>Modelo do id informado</returns>
        TModel GetEntityById(Guid id);

        /// <summary>
        /// Retorna uma lista de acordo com o filtro dinamico informado
        /// </summary>
        /// <param name="whereConditions">Objeto dinamico para filtro</param>
        /// <returns>Lista de acordo com o filtro dinamico</returns>
        List<TModel> GetByDynamicFilter(object whereConditions);

        /// <summary>
        /// Obtem modelo pelo id informado por tipo dinamico
        /// </summary>
        /// <typeparam name="IdType">Tipo do id</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TModel GetEntityById<IdType>(IdType id);

        /// <summary>
        /// Retorna o primeiro retorno de acordo com filtro dinamico
        /// </summary>
        /// <param name="whereConditions">Objeto dinamico para filtro</param>
        /// <returns>Lista de acordo com o filtro dinamico</returns>
        TModel GetFirstEntityByDynamicFilter(object whereConditions);

        /// <summary>
        /// Obtem lista com todos os objetos da entidade salvos em banco
        /// </summary>
        /// <returns>Lista de entidades</returns>
        List<TModel> GetAll();

        /// <summary>
        /// Remove entidade em banco
        /// </summary>
        /// <param name="entity">Modelo informado</param>
        /// <returns>Resultado da ação</returns>
        int Delete(TModel model);
    }
}