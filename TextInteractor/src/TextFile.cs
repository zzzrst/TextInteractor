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
        /// This methods modifies the textfile by replacing text in the text file.
        /// </summary>
        /// <param name="replaceType"> 0 means to replace once, 1 means to replaceAll, 2 means to replaceLine,3 means to replace regex<see cref="int"/>.</param>
        /// <param name="args">The args<see cref="string"/>.</param>
        /// <returns> <code>true</code> if the modification was successful.<see cref="bool"/>.</returns>
        public abstract bool Modify(int replaceType, string args);

        /// <summary>
        /// This method compares the text file with another text file.
        /// </summary>
        /// <param name="txtFile">Text file to compare.</param>
        /// <param name="resultFilePath">Text file that shows the comparison result when different. If there is a file name conflict, we will replace the file.</param>
        /// <param name="ignoreWhitespace">Set to true if we are going to ignore whitespace during comparison. Default is false.</param>
        /// <param name="caseInsensitive">Set to true if we are going to perform case insensitive comparison. Default is false.</param>
        /// <returns><code>true</code> if the contents of the text files match.</returns>
        public abstract bool Compare(TextFile txtFile, string resultFilePath, bool ignoreWhitespace = false, bool caseInsensitive = false);

        /// <summary>
        /// This method compares the text file with another text file specifying the top left and bottom right coordinates.
        /// If we consider the file to be a 2D matrix of characters, the startingLine and startingIndex would be the top left corner (x, y) respectively.
        /// The endingLine and endingIndex would be the bottom right coordinate on the bottom right corner.
        /// </summary>
        /// <param name="txtFile">The textfile that you want to compare with.<see cref="TextFile"/>.</param>
        /// <param name="resultFilePath">Text file that shows the comparison result when different.</param>
        /// <param name="startingLine">The line index to start the comparison.<see cref="int"/>.</param>
        /// <param name="startingIndex">The character index to start the comparison.<see cref="int"/>.</param>
        /// <param name="endingLine">The line index to stop the comparison.<see cref="int"/>.</param>
        /// <param name="endingIndex">The character index to end the comparison.<see cref="int"/>.</param>
        /// <param name="ignoreWhitespace">Set to true if we are going to ignore whitespace during comparison. Default is false.</param>
        /// <param name="caseInsensitive">Set to true if we are going to perform case insensitive comparison. Default is false.</param>
        /// <returns><code>true</code> if the contents of the text file match.</returns>
        public abstract bool Compare(TextFile txtFile, string resultFilePath, int startingLine, int startingIndex, int endingLine, int endingIndex, bool ignoreWhitespace = false, bool caseInsensitive = false);
    }
}
