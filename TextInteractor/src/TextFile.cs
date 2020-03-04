// <copyright file="TextFile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace TextInteractor
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This class represents a text file with data.
    /// </summary>
    public abstract class TextFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        /// <param name="filePath">The path to the textFile.</param>
        public TextFile(string filePath)
            : this(filePath, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        /// <param name="filePath">The path to the textFile.</param>
        /// <param name="logger">Logger that is passed in.</param>
        public TextFile(string filePath, ILogger logger)
        {
            this.Logger = logger;
            this.FilePath = filePath;
            TextFileLogHelper.Logger = logger;
        }

        /// <summary>
        /// Gets or sets the FilePath
        /// The full file path of the text file.
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file has been opened / not.
        /// </summary>
        public bool Opened { get; protected set; }

        /// <summary>
        /// Gets or sets the logger that will be used by TextFile.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// This method opens the text file for reading and writing. If file is already open, nothing will happen.
        /// </summary>
        /// <returns><code>true</code> if the text file successfully opens.</returns>
        public abstract bool Open();

        /// <summary>
        /// This method closes the text file for reading and writing. If file not open, nothing will happen.
        /// </summary>
        /// <returns><code>true</code> if the text file successfully closes.</returns>
        public abstract bool Close();

        /// <summary>
        /// This method reads a line of characters in the file and returns the data as a string.
        /// </summary>
        /// <returns>A string from the text file.</returns>
        public abstract string ReadLine();

        /// <summary>
        /// This method resets the reader back to the beginning of the text file.
        /// </summary>
        public abstract void RestartReading();

        /// <summary>
        /// This methods returns whether or not the reader is at the end of the text file.
        /// </summary>
        /// <returns><code>true</code> if the file has been finished reading.</returns>
        public abstract bool FinishedReading();

        /// <summary>
        /// This method tries to find the expected string in the specified line.
        /// </summary>
        /// <param name="expectedString"> String to be found.</param>
        /// <param name="line"> Line in the file to check, index starts at 1.</param>
        /// <returns><code>true</code> if the expected string is found at line.</returns>
        public abstract bool Find(string expectedString, int line);

        /// <summary>
        /// Replaces all occurances of the given string with the replacement string in the order they appear.
        /// </summary>
        /// <param name="toReplace">The string to replace.</param>
        /// <param name="replaceWith">What string to replace with.</param>
        /// <param name="numberOfTimes">Number of occurances to replace. Any number less than 0 is replace all. Default is replace all.</param>
        /// <returns><code>true</code> if the modification was successful.<see cref="bool"/>.</returns>
        public abstract bool ReplaceOccurances(string toReplace, string replaceWith, int numberOfTimes = -1);

        /// <summary>
        /// Replaces all the given lines with the replacement string.
        /// </summary>
        /// <param name="lines">The lines to replace.</param>
        /// <param name="replaceWith">The string to replace each lines with.</param>
        /// <returns><code>true</code> if the modification was successful.</returns>
        public abstract bool ReplaceLine(int[] lines, string replaceWith);

        /// <summary>
        /// This method compares the text file with another text file.
        /// </summary>
        /// <param name="txtFile">Text file to compare.</param>
        /// <param name="resultFilePath">Text file that shows the comparison result when different. If there is a file name conflict, we will replace the file.</param>
        /// <param name="ignoreWhitespace">Set to true if we are going to ignore whitespace during comparison. Default is false.</param>
        /// <param name="caseInsensitive">Set to true if we are going to perform case insensitive comparison. Default is false.</param>
        /// <returns><code>true</code> if the contents of the text files match.</returns>
        public abstract bool Compare(TextFile txtFile, string resultFilePath, bool ignoreWhitespace = false, bool caseInsensitive = false);
    }
}
