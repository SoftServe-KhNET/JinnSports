using System;
using System.Runtime.Serialization;

namespace JinnSports.BLL.Exceptions
{
    [Serializable]
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException()
        {
        }

        public TeamNotFoundException(string message) : base(message)
        {
        }

        public TeamNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TeamNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
