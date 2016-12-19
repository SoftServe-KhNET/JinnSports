using System;
using System.Runtime.Serialization;

namespace JinnSports.Parser.App.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException()
        {

        }

        public ParseException(string message) : base(message)
        {

        }

        public ParseException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public ParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
