namespace LogBook.Objects
{
    /// <summary>
    /// A storage class that contains the necessary details of a log file.
    /// </summary>
    public class LogFileSignature
    {
        /// <summary>
        /// The number of fields to the left of the Message field within the Log4Net mask.
        /// </summary>
        public int NumberOfFieldsBeforeMessage { get; set; }
        /// <summary>
        /// The string used to delimiter the file's fields.
        /// </summary>
        public string Delimiter { get; set; }
        /// <summary>
        /// The physical disk location of the Log File.
        /// </summary>
        public string FileLocation { get; set; }
        /// <summary>
        /// The mask used to determine the makeup of the conversion pattern for the Log File.
        /// </summary>
        public string FileMask { get; set; }
        /// <summary>
        /// Determines if the Log file contains a header string to separate out.
        /// </summary>
        public bool HasHeader
        {
            get { return !string.IsNullOrEmpty(Header); }
        }
        /// <summary>
        /// The string value of the header line within the log file.
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// Determines if the Log file contains a footer value to separate out.
        /// </summary>
        public bool HasFooter
        {
            get { return !string.IsNullOrEmpty(Footer); }
        }
        /// <summary>
        /// The string value of the footer line within the log file.
        /// </summary>
        public string Footer { get; set; }
    }
}
