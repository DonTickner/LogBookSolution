using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace LogBook.Akka
{
    public static class Messages
    {
        #region Error messages

        /// <summary>
        /// Base class for signalling that user input was invalid
        /// </summary>
        public class InputError
        {
            public string Reason { get; private set; }

            public InputError(string reason)
            {
                Reason = reason;
            }
        }

        #endregion
    }
}
