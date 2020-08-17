using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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

            context.Response.StatusCode = (int)statusCode;


            var errorResponse = new ErrorResponse { Error = message };

            var selector = context.RequestServices.GetRequiredService<OutputFormatterSelector>();
            var writerFactory = context.RequestServices.GetRequiredService<IHttpResponseStreamWriterFactory>();
            var formatterContext = new OutputFormatterWriteContext(context, writerFactory.CreateWriter, typeof(ErrorResponse), errorResponse);


            var selectedFormatter = selector.SelectFormatter(formatterContext, Array.Empty<IOutputFormatter>(), new MediaTypeCollection());


            return selectedFormatter.WriteAsync(formatterContext);

        }
    }
}
