using System;
using System.Runtime.Serialization;

namespace JinnSports.Parser.App.Exceptions
{
    public class GetDataException : Exception
    {
        public GetDataException() : base()
        {

        }

        public GetDataException(string message) : base(message)
        {

        }

        public GetDataException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public GetDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
