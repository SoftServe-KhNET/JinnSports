using System;
using System.Runtime.Serialization;

namespace JinnSports.Parser.App.Exceptions
{
    public class WebResponseException : Exception
    {
        public WebResponseException() : base()
        {

        }

        public WebResponseException(string message) : base(message)
        {

        }

        public WebResponseException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public WebResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
