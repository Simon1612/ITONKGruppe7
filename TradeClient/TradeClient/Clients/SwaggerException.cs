using System;
using System.Collections.Generic;

namespace TradeClient.Clients
{

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "11.17.10.0 (NJsonSchema v9.10.49.0 (Newtonsoft.Json v9.0.0.0))")]
    public class SwaggerException : Exception
    {
        public int StatusCode { get; }

        public string Response { get; }

        public Dictionary<string, IEnumerable<string>> Headers { get; }

        public SwaggerException(string message, int statusCode, string response, Dictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }
}
