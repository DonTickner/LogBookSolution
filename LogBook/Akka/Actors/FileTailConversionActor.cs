using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using Akka.Actor;
using LogBook.Objects;

namespace LogBook.Akka.Actors
{
    public class FileTailConversionActor: UntypedActor
    {
        /// <summary>
        /// Message class for triggering a conversion of the String for writing away
        /// </summary>
        public class ConvertString
        {
            public string StringToConvert { get; private set; }

            public ConvertString(string stringToConvert)
            {
                StringToConvert = stringToConvert;
            }
        }

        private readonly IActorRef _dataUpdateActorRef;
        private readonly LogFileSignature _logFileSignature;

        public FileTailConversionActor(IActorRef dataUpdateActor, LogFileSignature logFileSignature)
        {
            _dataUpdateActorRef = dataUpdateActor;
            _logFileSignature = logFileSignature;
        }

        protected override void OnReceive(object message)
        {
            if (message is ConvertString write)
            {
                if (!string.IsNullOrEmpty(write.StringToConvert))
                {
                    _dataUpdateActorRef.Tell(new DataUpdateActor.WriteMessage(ConvertData(write.StringToConvert)));
                }
            }
        }

        private string[] ConvertData(string textToWrite)
        {
            List<string> fieldsToWrite = new List<string>();

            if (_logFileSignature.HasHeader &&
                textToWrite.Contains(_logFileSignature.Header))
            {
                return MapStringToMessageField(textToWrite).ToArray();
            }

            if (_logFileSignature.HasFooter &&
                     textToWrite.Contains(_logFileSignature.Footer))
            {
                return MapStringToMessageField(textToWrite).ToArray();
            }

            while (textToWrite.Contains("  "))
            {
                textToWrite = textToWrite.Replace("  ", " ");
            }

            int delimiters = 0;
            if (_logFileSignature.Delimiter != string.Empty &&
                _logFileSignature.NumberOfFieldsBeforeMessage > 1)
            {
                delimiters = textToWrite.Length - textToWrite.Replace(_logFileSignature.Delimiter, "").Length;
            }
            
            if (delimiters < _logFileSignature.NumberOfFieldsBeforeMessage)
            {
                return MapStringToMessageField(textToWrite).ToArray();
            }

            for (int i = 0; i < _logFileSignature.NumberOfFieldsBeforeMessage; i++)
            {
                int indexOfDelim = textToWrite.IndexOf(_logFileSignature.Delimiter, StringComparison.Ordinal);
                if (indexOfDelim < 0)
                {
                    return MapStringToMessageField(textToWrite).ToArray();
                }
                
                string field = textToWrite.Substring(0, indexOfDelim);
                fieldsToWrite.Add(field);
                textToWrite = textToWrite.Substring(indexOfDelim + 1, textToWrite.Length - indexOfDelim - 1);
            }
            fieldsToWrite.Add(textToWrite);
            
            return fieldsToWrite.ToArray();
        }

        private List<string> MapStringToMessageField(string textToWrite)
        {
            List<string> fieldsToWrite = new List<string>();
            for (int i = 0; i < _logFileSignature.NumberOfFieldsBeforeMessage + 1; i++)
            {
                fieldsToWrite.Add(string.Empty);
            }
            fieldsToWrite[fieldsToWrite.Count - 1] = textToWrite;
            return fieldsToWrite;
        }
    }
}
