// <copyright file="TextFile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    /// <summary>
    /// This class represents a text file with data.
    /// </summary>
    public abstract class TextFile
    {
        /// <summary>
        /// Gets or sets the FilePath
        /// The full file path of the text file.
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether Opened
        /// Boolean declaring whether or not the file is opened.
        /// </summary>
        public bool Opened { get; protected set; }

        /// <summary>
        /// This method opens the text file for reading and writing.
        /// </summary>
        /// <returns><code>true</code> if the text file successfully opens.</returns>
        public abstract bool Open();

        /// <summary>
        /// This method closes the text file for reading and writing.
        /// </summary>
        /// <returns><code>true</code> if the text file successfully closes.</returns>
        public abstract bool Close();

        /// <summary>
        /// This method compares the text file with another text file.
        /// </summary>
        /// <param name="txtFile">Text file to compare.</param>
        /// <returns><code>true</code> if the contents of the text files match.</returns>
        public abstract bool Compare(TextFile txtFile);

        /// <summary>
        /// The Compare.
        /// </summary>
        /// <param name="txtFile">The txtFile<see cref="TextFile"/>.</param>
        /// <param name="startingLine">The startingLine<see cref="int"/>.</param>
        /// <param name="startingIndex">The startingIndex<see cref="int"/>.</param>
        /// <param name="endingLine">The endingLine<see cref="int"/>.</param>
        /// <param name="endingIndex">The endingIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Compare(TextFile txtFile, int startingLine, int startingIndex, int endingLine, int endingIndex);

        /// <summary>
        /// This method compares the text file with another text file.
        /// </summary>
        /// <param name="txtFile">Text file to compare.</param>
        /// <param name="ignoreWhitespace">if true, ignores surrounding whitespace while comparing.</param>
        /// <param name="caseInsensitive">if true, ignores alphabetical case while comparing.</param>
        /// <returns><code>true</code> if the contents of the text files match.</returns>
        public abstract bool Compare(TextFile txtFile, bool ignoreWhitespace = false, bool caseInsensitive = false);

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
        /// <param name="replaceType"> 0 means to replace once, 1 means to replaceAll, 2 means to replaceLine<see cref="int"/>.</param>
        /// <param name="args">The args<see cref="string"/>.</param>
        /// <returns> <code>true</code> if the modification was successful. <see cref="bool"/>.</returns>
        public abstract bool Modify(int replaceType, string args);

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
    }
}
