using System.Text.Json;

namespace AAPS.Api.Middlewares
{
    public class AutenticacaoMiddleware
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly RequestDelegate _next;

        public AutenticacaoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await HandleUnauthorizedResponse(context);
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await HandleForbiddenResponse(context);
            }
        }

        private static async Task HandleUnauthorizedResponse(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                sucesso = false,
                mensagem = "Usuário não autenticado. Faça login e tente novamente.",
                status = 401
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleForbiddenResponse(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                sucesso = false,
                mensagem = "Usuário sem permissão para acessar este recurso.",
                status = 403
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}