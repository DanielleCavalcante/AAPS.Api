using AAPS.Api.Responses;
using System.Net;
using System.Text.Json;

namespace AAPS.Api.Middlewares
{
    public class ExcecoesMiddleware
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly RequestDelegate _next;

        public ExcecoesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var erroResponse = ApiResponse<object>.ErroResponse(
                new List<string> { ex.Message },
                "Erro interno no servidor."
            );

            var jsonResponse = JsonSerializer.Serialize(erroResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await response.WriteAsync(jsonResponse);
        }
    }
}
