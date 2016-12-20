using System;
using System.Runtime.Serialization;

namespace JinnSports.Parser.App.Exceptions
{
    public class JsonDeserializeException : Exception
    {
        public JsonDeserializeException() : base()
        {

        }

        public JsonDeserializeException(string message) : base(message)
        {

        }

        public JsonDeserializeException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public JsonDeserializeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
