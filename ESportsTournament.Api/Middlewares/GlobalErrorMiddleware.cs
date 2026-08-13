using System.Net;
using System.Text.Json;

namespace ESportsTournament.Api.Middlewares
{
    public class GlobalErrorMiddleware
    {
        // O RequestDelegate representa o "próximo passo" no túnel da requisição
        private readonly RequestDelegate _next;

        public GlobalErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Tenta deixar a requisição seguir o seu fluxo normal (ir para o Controller)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Se qualquer erro estourar no sistema inteiro, ele cai aqui!
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Por padrão, dizemos que o erro é 500 (Erro Interno)
            var statusCode = HttpStatusCode.InternalServerError;
            var mensagem = "Ocorreu um erro interno no servidor.";

            // Se o erro for a nossa Regra de Negócio (InvalidOperationException), mudamos para 400
            if (exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                mensagem = exception.Message; // Pega a mensagem exata que escrevemos no Service
            }

            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Forbidden; // 403
                mensagem = exception.Message;
            }

            // Configura a resposta para ser em formato JSON
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Monta o objeto de erro que o Swagger/Front-end vai receber
            var response = new
            {
                Mensagem = mensagem,
                // Se for erro 500, mostramos o detalhe. Se for 400, o detalhe fica vazio.
                Detalhe = statusCode == HttpStatusCode.InternalServerError ? exception.Message : null
            };

            // Transforma o objeto em texto JSON e devolve para o usuário
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
