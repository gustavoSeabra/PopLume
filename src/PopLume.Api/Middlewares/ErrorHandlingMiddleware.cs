using PopLume.Application.Dtos;
using System.Net;
using System.Text.Json;

namespace PopLume.Api.Middlewares
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro inesperado ao processar a requisição.");
                await TratarExceptionAsync(context);
            }
        }

        private static Task TratarExceptionAsync(HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = ResultadoDto<bool>.RetornaErro("Um erro inesperado ocorreu.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
