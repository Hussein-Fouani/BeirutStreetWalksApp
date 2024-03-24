using Microsoft.AspNetCore.Http;
using System.Net;

namespace BeirutWalksWebApi.GlobalHandling
{
    public class GlobalHandler
    {
        private readonly RequestDelegate next;

        public GlobalHandler(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvodeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }catch(Exception ex)
            {
                Guid id = Guid.NewGuid();
                var response = new { id = id, message = "Something went wrong" };
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var error = new
                {
                    Id = id,
                    ErrorMessage = response
                };
                await context.Response.WriteAsJsonAsync(error);
            }

        }
    }
}
