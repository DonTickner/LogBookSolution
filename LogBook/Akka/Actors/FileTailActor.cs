using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Akka.Actor;
using LogBook.Objects;

namespace LogBook.Akka.Actors
{
    class FileTailActor: UntypedActor
    {
        public class FileError
        {
            public string FileName { get; private set; }
            public string Reason { get; private set; }

            public FileError(string fileName, string reason)
            {
                FileName = fileName;
                Reason = reason;
            }
        }

        /// <summary>
        /// Message class for triggering the initial full read from the Log File
        /// </summary>
        private class Read
        {
            public string Text { get; private set; }

            public Read(string text)
            {
                Text = text;
            }
        }

        public class StartRead
        {
        }

        public class GoNowAndDieInWhateverMannerSeemsBestToYou
        {
        }

        private readonly IActorRef _fileTailConversionActor;
        private readonly IActorRef _dataUpdateActor;
        private readonly StreamReader _fileStreamReader;
        private readonly FileWatcher.FileWatcher _fileWatcher;

        public FileTailActor(DataTable resultsDataTable, LogFileSignature logFileSignature)
        {
            _dataUpdateActor = Context.ActorOf(Props.Create(() => new DataUpdateActor(resultsDataTable)));
            _fileTailConversionActor = Context.ActorOf(Props.Create(() => new FileTailConversionActor(_dataUpdateActor, logFileSignature)));

            _fileWatcher = new FileWatcher.FileWatcher(Self, Path.GetFullPath(logFileSignature.FileLocation));
            _fileWatcher.Start();

            string fullPath = Path.GetFullPath(logFileSignature.FileLocation);

            Stream fileStream = new FileStream(fullPath,
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _fileStreamReader = new StreamReader(fileStream, Encoding.UTF8);

            Self.Tell(new StartRead());
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case StartRead _:
                {
                    var newLine = _fileStreamReader.ReadLine();
                    if (null != newLine)
                    {
                        _fileWatcher.Observe = false;
                        Self.Tell(new Read(newLine));
                    }
                    break;
                }
                case Read read:
                {
                    _fileTailConversionActor.Tell(new FileTailConversionActor.ConvertString(read.Text));
                    var text = _fileStreamReader.ReadLine();
                    if (null != text)
                    {
                        Self.Tell(new Read(text));
                    }
                    else
                    {
                        _fileWatcher.Observe = true;
                    }
                    break;
                }
                case GoNowAndDieInWhateverMannerSeemsBestToYou _:
                {
                    _dataUpdateActor.Tell(new GoNowAndDieInWhateverMannerSeemsBestToYou());
                    Context.Stop(_fileTailConversionActor);

                    _fileStreamReader.Dispose();
                    _fileWatcher.Dispose();
                    Context.Stop(Self);
                    break;
                }
            }
        }
    }
}
