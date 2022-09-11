using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.ErrorHandling
{
    public class ExceptionHandler
    {
        public RequestDelegate RequestHandler { get; }
        public ExceptionHandler()
        {
            RequestHandler = new RequestDelegate(this.HandleException);
        }

        private async Task HandleException(HttpContext httpContext)
        {
            var exceptionHandler = httpContext.Features.Get<IExceptionHandlerFeature>();
            await WriteExceptionMessage(httpContext, exceptionHandler.Error);
        }

        private async Task WriteExceptionMessage(HttpContext httpContext, Exception exception)
        {
            switch (exception)
            {
                case BusinessException businessException:
                    await WriteResponse(httpContext, (int)businessException.StatusCode, businessException.Message);
                    break;
                default:
                    await WriteResponse(httpContext, (int)HttpStatusCode.InternalServerError, "Internal Server Error");
                    break;
            }

        }

        private async Task WriteResponse(HttpContext httpContext, int statusCode, string message)
        {
            httpContext.Response.StatusCode = statusCode;
            if (message != null)
                await httpContext.Response.WriteAsync(message);

        }
    }
}
