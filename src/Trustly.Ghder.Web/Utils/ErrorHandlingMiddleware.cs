using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trustly.Ghder.Core;

namespace Trustly.Ghder.Web.Utils
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            string message;

            if (ex is DomainException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = "Internal Server Error";
            }


            var result = JsonConvert.SerializeObject(new ErrorResponse { Error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
