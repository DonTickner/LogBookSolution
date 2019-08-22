using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Xml.Linq;
using LogBook.Exceptions;
using LogBook.Objects.Forms;

namespace LogBook.Objects
{
    /// <summary>
    /// The Factory class for creating new <see cref="Page"/> forms to display Log file contents
    /// </summary>
    public class PageFactory
    {
        private readonly MainWindow _parentMainWindow;

        public PageFactory(MainWindow parentMainWindow)
        {
            _parentMainWindow = parentMainWindow;
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> that targets a specific Log4Net config file.
        /// </summary>
        public List<Page> CreateNewLog4NetPage(string log4NetConfigFileLocation)
        {
            List<LogFileSignature> logFilesToOpen = CreateLogFileMask(RetrieveLog4NetConfigFileInformation(log4NetConfigFileLocation));

            if (!logFilesToOpen.Any())
            {
                throw new InvalidLog4NetConfigException("Unable to read .config file. Unknown error occurred. Please review structure of the file.");
            }

            List<Page> openLogFiles = new List<Page>();
            foreach (LogFileSignature logFileSignature in logFilesToOpen)
            {
                if(!File.Exists(logFileSignature.FileLocation))
                {
                    throw new FileNotFoundException(logFileSignature.FileLocation);
                }

                Page blankPage = new Page()
                {
                    ParentMainWindow = _parentMainWindow,
                    LogFileSignature = logFileSignature
                };
                blankPage.Setup(true);
                openLogFiles.Add(blankPage);
            }
            
            return openLogFiles;
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> that targets a generic text file.
        /// </summary>
        public List<Page> CreateTextPage(string textFileLocation)
        {
            List<Page> openLogFiles = new List<Page>();
            if (!File.Exists(textFileLocation))
            {
                throw new FileNotFoundException(textFileLocation);
            }

            Page blankPage = new Page()
            {
                ParentMainWindow = _parentMainWindow,
                LogFileSignature = new LogFileSignature()
                {
                    Delimiter = "",
                    FileLocation = textFileLocation,
                    NumberOfFieldsBeforeMessage = 0
                }
            };
            blankPage.Setup(true);
            openLogFiles.Add(blankPage);

            return openLogFiles;
        }

        /// <summary>
        /// Retrieves the Log File information from the log4net .config file passed.
        /// </summary>
        /// <param name="logLocation">The physical file location of the log4net .config file</param>
        private List<LogFileSignature> RetrieveLog4NetConfigFileInformation(string logLocation)
        {
            FileStream stream = new FileStream(logLocation, FileMode.Open, FileAccess.Read);
            XDocument logFile = XDocument.Load(stream);
            stream.Dispose();

            if (!logFile.Elements("log4net").Any())
            {
                throw new InvalidLog4NetConfigException("Unable to locate <log4net> node within config file. Please review file structure and try again.");
            }

            if (!logFile.Elements("log4net").Elements("appender").Any())
            {
                throw new InvalidLog4NetConfigException("Unable to locate an <appender> node within <log4net> node. Please review file structure and try again.");
            }

            var rollingFileAppenders = logFile
                .Elements("log4net")
                .Elements("appender")
                .Where(a => a.Attribute("type")?.Value == "log4net.Appender.RollingFileAppender");

            if (!rollingFileAppenders.Any())
            {
                throw new InvalidLog4NetConfigException("Unable to locate an <appender> of type 'log4net.Appender.RollingFileAppender' within <log4net> node. Unable to proceed.");
            }

            List<LogFileSignature> logFileSignatures = new List<LogFileSignature>();
            foreach (XElement fileAppender in rollingFileAppenders)
            {
                if (!fileAppender.Descendants().Any())
                {
                    throw new InvalidLog4NetConfigException("<appender> node of type 'log4net.Appender.RollingFileAppender' has no descendent nodes. Unable to proceed.");
                }

                string fileLocation = string.Empty;
                string fileMask = string.Empty;
                string header = string.Empty;
                string footer = string.Empty;
                foreach (XElement descendant in fileAppender.Descendants())
                {
                    switch (descendant.Name.ToString().ToLower())
                    {
                        case "file":
                        {
                            fileLocation = descendant.Attribute("value")?.Value;
                            break;
                        }

                        case "conversionpattern":
                        {
                            fileMask = descendant.Attribute("value")?.Value;
                            break;
                        }

                        case "header":
                        {
                            header = descendant.Attribute("value")?.Value;
                            break;
                        }

                        case "footer":
                        {
                            footer = descendant.Attribute("value")?.Value;
                            break;
                        }

                        case "param":
                        {
                            switch (descendant.Attribute("name")?.Value.ToLower())
                            {
                                case "file":
                                {
                                    if (fileLocation == string.Empty)
                                    {
                                        fileLocation = descendant.Attribute("value")?.Value;
                                    }

                                    break;
                                }

                                case "conversionpattern":
                                {
                                    if (fileMask == string.Empty)
                                    {
                                        fileMask = descendant.Attribute("value")?.Value;
                                    }
                                    break;
                                }

                                case "header":
                                {
                                    if (header == string.Empty)
                                    {
                                        header = descendant.Attribute("value")?.Value;
                                    }
                                    break;
                                }

                                case "footer":
                                {
                                    if (footer == string.Empty)
                                    {
                                        footer = descendant.Attribute("value")?.Value;
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }

                if (fileLocation != string.Empty)
                {
                    logFileSignatures.Add(new LogFileSignature()
                    {
                        FileLocation = LocateCurrentLogFile(fileLocation),
                        FileMask = fileMask,
                        Header = header,
                        Footer = footer
                    });
                }
                else
                {
                    throw new InvalidLog4NetConfigException("Unable to locate rolling file appender 'file' descendent, or its value is empty. Unable to identify log file location. Unable to proceed.");
                }
            }

            return logFileSignatures;
        }

        /// <summary>
        /// Creates a FileMask based on a Log4Net .config file.
        /// </summary>
        /// <param name="signatures"></param>
        /// <returns></returns>
        private List<LogFileSignature> CreateLogFileMask(List<LogFileSignature> signatures)
        {
            foreach (LogFileSignature signature in signatures)
            {
                string fileMask = signature.FileMask;
                if (fileMask == string.Empty)
                {
                    break;
                }

                int numberOfFieldsBeforeMessage = 0;
                bool foundMessage = false;
                for (int i = 0; i < fileMask.Length - 1; i++)
                {
                    if (!fileMask[i].Equals('%'))
                    {
                        continue;
                    }

                    if (fileMask[i + 1].Equals('%'))
                    {
                        i += 2;
                        continue;
                    }

                    string currentField = string.Empty;
                    bool innerOpen = false;
                    for (int x = i + 1; x < fileMask.Length; x++)
                    {
                        if (char.IsDigit(fileMask[x]))
                        {
                            continue;
                        }

                        currentField += fileMask[x];

                        if (string.Equals(currentField, "m", StringComparison.InvariantCulture))
                        {
                            foundMessage = true;
                            break;
                        }

                        if (fileMask[x].Equals('{'))
                        {
                            innerOpen = true;
                            continue;
                        }

                        if (fileMask[x].Equals(' ') &&
                            !innerOpen)
                        {
                            currentField = currentField.Substring(0, currentField.Length - 1);

                            if (string.Equals(currentField, "d") ||
                                string.Equals(currentField, "date"))
                            {
                                numberOfFieldsBeforeMessage++;
                            }

                            if (currentField[0].Equals('d') &&
                                currentField.Contains("{"))
                            {
                                int spaces = currentField.Count(c => c.Equals(' '));
                                numberOfFieldsBeforeMessage += spaces;
                            }
                            else
                            {
                                numberOfFieldsBeforeMessage++;
                            }
                            
                            i = x;
                            break;
                        }

                        if (fileMask[x].Equals('}'))
                        {
                            innerOpen = false;
                        }
                    }

                    if (foundMessage)
                    {
                        break;
                    }
                }

                signature.NumberOfFieldsBeforeMessage = numberOfFieldsBeforeMessage;
                signature.Delimiter = " ";
            }

            return signatures;
        }

        /// <summary>
        /// Retrieves the directory information from the passed file string, and identifies the most recent applicable log file for loading.
        /// </summary>
        /// <param name="file">The 'file' value from the log4net config file.</param>
        private string LocateCurrentLogFile(string file)
        {
            char escapedBackSlash = '\\';
            int locationOfBeginningOfFileName = file.LastIndexOf(escapedBackSlash) + escapedBackSlash.ToString().Length;
            string directoryPath = file.Substring(0, locationOfBeginningOfFileName);
            string log4NetFileNameMask = file.Substring(locationOfBeginningOfFileName);

            string[] log4NetLogFiles = Directory.GetFiles(directoryPath);

            if (!log4NetLogFiles.Any())
            {
                throw new InvalidLog4NetConfigException($"Indicated directory '{directoryPath}' contains no files.");
            }

            if (!log4NetLogFiles.Any(f => f.Contains(log4NetFileNameMask)))
            {
                throw new InvalidLog4NetConfigException($"Indicated directory '{directoryPath}' contains no files containing the log4net file name mask of '{log4NetFileNameMask}'");
            }

            // TODO: Implement smart file type detection in order validate files based on varying file types
            if (!log4NetLogFiles.Any(f => f.EndsWith(".txt")))
            {
                throw new InvalidLog4NetConfigException($"Indicated directory '{directoryPath}' contains no files of .TXT format to be loaded.");
            }


           DateTime mostRecentWriteTime = DateTime.MinValue;
            string mostRecentFile = string.Empty;

            foreach (string log4NetLogFile in log4NetLogFiles.Where(f => f.Contains(log4NetFileNameMask) 
                                                                        && f.EndsWith(".txt")))
            {
                DateTime currentLastWriteTime = File.GetLastWriteTime(log4NetLogFile);
                if (currentLastWriteTime > mostRecentWriteTime)
                {
                    mostRecentFile = log4NetLogFile;
                    mostRecentWriteTime = currentLastWriteTime;
                }
            }

            if (mostRecentFile == string.Empty ||
                mostRecentWriteTime == DateTime.MinValue)
            {
                throw new InvalidLog4NetConfigException($"Indicated directory '{directoryPath}' contains {log4NetLogFiles.Length} files, but none could be loaded.");
            }

            return mostRecentFile;
        }
    }
}
