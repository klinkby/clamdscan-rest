using System;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Klinkby.ClamREST
{
    [Serializable]
    public class ProblemException : Exception
    {
        public string Type { get; }
        public HttpStatusCode StatusCode { get; }
         
        public ProblemException(string type, string title, HttpStatusCode statusCode) : base(title)
        {
            Type = type;
            StatusCode = statusCode;
        }
        protected ProblemException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public ObjectResult ToProblem(ControllerBase controller)
        {
            return controller.Problem(type: Type, title: Message, statusCode: (int?)StatusCode ?? 500);
        }
    }
}
