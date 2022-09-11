using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ShoppingCart.Application.ErrorHandling
{
    public class BusinessException : Exception
    {
        private string _message;
        public BusinessException(string message, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            _message = message;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public override string Message => JsonSerializer.Serialize(new[] { _message });
    }
}
