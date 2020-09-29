using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GameWebApi
{

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }

            catch (NotFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Player not found!");
                return;
            }
        }
    }
}