using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace WebApi.Attribute
{
    /// <summary>
    /// Atributo global para selecionar header swagger
    /// </summary>
    public class CustomHeaderSwaggerAttribute : IOperationFilter
    {
        private const string HTTP_DELETE = "DELETE";

        /// <summary>
        /// Método invocado ao efetuar inclusão do swagger
        /// </summary>
        /// <param name="operation">Operação</param>
        /// <param name="context">Contexto</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if (context.ApiDescription.HttpMethod == HTTP_DELETE)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "ByPassConfirmation",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "Boolean"
                    }
                });
            }
        }
    }
}