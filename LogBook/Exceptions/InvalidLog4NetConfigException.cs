using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LogBook.Exceptions
{
    public class InvalidLog4NetConfigException: Exception
    {
        public InvalidLog4NetConfigException()
        {
        }

        public InvalidLog4NetConfigException(string message) : base(message)
        {
        }

        public InvalidLog4NetConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidLog4NetConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
