using System;
using System.Runtime.Serialization;

namespace JinnSports.Parser.App.Exceptions
{
    public class SaveDataException : Exception
    {
        public SaveDataException() : base()
        {

        }

        public SaveDataException(string message) : base(message)
        {

        }

        public SaveDataException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public SaveDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
