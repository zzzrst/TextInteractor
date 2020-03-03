// <copyright file="TextInteractor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="TextInteractor" />.
    /// </summary>
    public class TextInteractor : TextFile
    {
        /// <summary>
        /// Defines the reader.
        /// </summary>
        private StreamReader reader;

        /// <summary>
        /// Defines the log.
        /// </summary>
        private List<string> log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInteractor"/> class.
        /// </summary>
        /// <param name="filePath">The path to the textFile.</param>
        public TextInteractor(string filePath)
            : this(filePath, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInteractor"/> class.
        /// </summary>
        /// <param name="filePath">The path to the textFile.</param>
        /// <param name="logger">Logger that is passed in.</param>
        public TextInteractor(string filePath, ILogger logger)
            : base(filePath, logger)
        {
            this.reader = null;
            this.log = new List<string>();
        }

        /// <inheritdoc/>
        public override bool Open()
        {
            if (this.Opened)
            {
                return true;
            }

            try
            {
                this.reader = new StreamReader(this.FilePath);
                this.Opened = true;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex.ToString());
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Close()
        {
            if (this.Opened)
            {
                try
                {
                    this.reader.Close();
                    this.reader.Dispose();
                    this.Opened = false;
                    this.Logger?.LogInformation("TextInteractor: file has been closed: " + this.FilePath);
                }
                catch (Exception ex)
                {
                    this.Logger?.LogError(ex.ToString());
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override string ReadLine()
        {
            if (!this.Opened)
            {
                this.Open();
            }

            return this.reader.EndOfStream ? null : this.reader.ReadLine();
        }

        /// <inheritdoc/>
        public override void RestartReading()
        {
            if (!this.Opened)
            {
                this.Open();
            }

            this.reader.DiscardBufferedData();
            this.reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        /// <inheritdoc/>
        public override bool FinishedReading()
        {
            if (!this.Opened)
            {
                this.Open();
            }

            return this.reader.EndOfStream;
        }

        /// <inheritdoc/>
        public override bool Find(string expectedString, int lineNum)
        {
            this.Open();
            this.RestartReading();

            string line;
            int lineIndex = 1;

            while ((line = this.reader.ReadLine()) != null && lineIndex <= lineNum)
            {
                if (lineIndex == lineNum)
                {
                    return line.Contains(expectedString);
                }

                lineIndex++;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool ReplaceOccurances(string toReplace, string replaceWith, int numberOfTimes = -1)
        {
            var regex = new Regex(toReplace);
            this.Open();
            using (this.reader)
            {
                using (var tempFile = new StreamWriter(this.FilePath + ".tmp"))
                {
                    string line;
                    while ((line = this.reader.ReadLine()) != null)
                    {
                        if (numberOfTimes > 0)
                        {
                            int matchs = regex.Matches(line).Count;
                            line = regex.Replace(line, replaceWith, numberOfTimes);
                            numberOfTimes = Math.Max(numberOfTimes - matchs, 0);
                        }
                        else if (numberOfTimes < 0)
                        {
                            line = regex.Replace(line, replaceWith);
                        }

                        tempFile.WriteLine(line);
                    }
                }

                this.reader.Close();
                File.Delete(this.FilePath);
                File.Move(this.FilePath + ".tmp", this.FilePath);
            }

            // reopens the file to restore the stream reader.
            this.Close();
            this.Open();

            return true;
        }

        /// <inheritdoc/>
        public override bool ReplaceLine(int[] lines, string replaceWith)
        {
            this.Open();
            using (this.reader)
            {
                using (var tempFile = new StreamWriter(this.FilePath + ".tmp"))
                {
                    string line;
                    int lineIndex = 1;
                    while ((line = this.reader.ReadLine()) != null)
                    {
                        if (lines.Contains(lineIndex))
                        {
                            line = replaceWith;
                        }

                        tempFile.WriteLine(line);
                        lineIndex++;
                    }
                }

                this.reader.Close();
                File.Delete(this.FilePath);
                File.Move(this.FilePath + ".tmp", this.FilePath);
            }

            // reopens the file to restore the stream reader.
            this.Close();
            this.Open();

            return true;
        }

        /// <inheritdoc/>
        public override bool Compare(TextFile txtFile, string resultFilePath, bool ignoreWhitespace = false, bool caseInsensitive = false)
        {
            bool areEqual;
            areEqual = this.Compare(txtFile, resultFilePath, 1, 0, int.MaxValue, int.MaxValue, ignoreWhitespace, caseInsensitive);
            return areEqual;
        }

        /// <inheritdoc/>
        public override bool Compare(TextFile txtFile, string resultFilePath, int startingLine, int startingIndex, int endingLine, int endingIndex, bool ignoreWhitespace = false, bool caseInsensitive = false)
        {
            bool areEqual = true;
            string errorMsg = string.Empty;

            // Open both files
            this.Open();
            txtFile.Open();

            // restart the readers of both files
            this.RestartReading();
            txtFile.RestartReading();

            TextFileLogHelper.LogComparisonBeginning(this, txtFile, ignoreWhitespace, caseInsensitive);

            // read until the end of file A
            int lineNumber = 1;
            string lineA;
            bool unequalLineCount = false;
            while ((lineA = this.ReadLine()) != null)
            {
                string lineB = txtFile.ReadLine();

                // check if file B still has lines to be read
                if (lineB == null)
                {
                    errorMsg = "File B has less lines than File A";
                    errorMsg += "\n   File B: " + (lineNumber - 1) + " lines";
                    areEqual = false;
                    unequalLineCount = true;
                    break;
                }

                // check when to stop comparing
                if (lineNumber <= endingLine)
                {
                    // if ignore whitespace is enabled
                    if (ignoreWhitespace)
                    {
                        lineA = Regex.Replace(lineA, @"\s+", string.Empty);
                        lineB = Regex.Replace(lineB, @"\s+", string.Empty);
                    }

                    // if case insensitivity is enabled
                    if (caseInsensitive)
                    {
                        lineA = lineA.ToUpper();
                        lineB = lineB.ToUpper();
                    }

                    // check if should start comparing.
                    if (lineNumber >= startingLine)
                    {
                        // Check if on first/last line for comparing.
                        if (lineNumber == startingLine)
                        {
                            lineA = lineA.Substring(startingIndex);
                            lineB = lineB.Substring(startingIndex);
                        }
                        else if (lineNumber == endingLine)
                        {
                            lineA = lineA.Substring(0, endingIndex);
                            lineB = lineB.Substring(0, endingIndex);
                        }

                        // check if line A matches line B in value
                        if (lineA != lineB)
                        {
                            int diffIndex = TextFileLogHelper.FirstDifferentChar(lineA, lineB);
                            errorMsg = "line " + lineNumber + " is not equal in both files";
                            errorMsg += "\n   File A: " + lineA;
                            errorMsg += "\n   File B: " + lineB;
                            errorMsg += "\n           " + string.Concat(Enumerable.Repeat(" ", diffIndex)) + "^";
                            this.log.Add(errorMsg);
                            areEqual = false;
                        }
                    }
                }

                ++lineNumber;
            }

            // Reads to the end of file A,
            // For the case of not reading to the end of A when comparing
            while (this.ReadLine() != null)
            {
                txtFile.ReadLine();
            }

            // if file B has less lines than A.
            if (unequalLineCount)
            {
                while (this.ReadLine() != null)
                {
                    ++lineNumber;
                }

                errorMsg += "\n   File A: " + lineNumber + " lines";
                this.log.Add(errorMsg);
            }

            // check if file B has already finished reading
            if (!txtFile.FinishedReading())
            {
                errorMsg = "File A has less lines than File B";
                errorMsg += "\n   File A: " + lineNumber + " lines";
                while (txtFile.ReadLine() != null)
                {
                    ++lineNumber;
                }

                errorMsg += "\n   File B: " + lineNumber + " lines";
                this.log.Add(errorMsg);
                areEqual = false;
            }

            TextFileLogHelper.LogComparisonEnd(this, areEqual, this.log);

            // restart the readers of both files again
            this.RestartReading();
            txtFile.RestartReading();

            // create compare log file if discrepancies exists
            if (this.log.Any())
            {
                this.CreateErrorLog(resultFilePath);
                this.Logger?.LogInformation("Compare log saved at " + resultFilePath);
            }

            return areEqual;
        }

        /// <summary>
        /// Find if string exist or not.
        /// </summary>
        /// <param name="expectedString">The string to be found.<see cref="string"/>.</param>
        /// <returns><code>true</code> if expectedString is found<see cref="bool"/>.</returns>
        public bool Find(string expectedString)
        {
            this.Open();

            using (this.reader)
            {
                string line = string.Empty;
                while ((line = this.reader.ReadLine()) != null)
                {
                    return line.Contains(expectedString);
                }
            }

            return false;
        }

        /// <summary>
        /// The FindAndCount.
        /// </summary>
        /// <param name="expectedString">The string to be found and counted.<see cref="string"/>.</param>
        /// <returns>The count of the expectedString in file <see cref="int"/>.</returns>
        public int FindAndCount(string expectedString)
        {
            this.Open();

            int count = 0;

            using (this.reader)
            {
                string line = string.Empty;
                while ((line = this.reader.ReadLine()) != null)
                {
                    if (line.Contains(expectedString))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// The LineExactMatch.
        /// </summary>
        /// <param name="expectedString">The expected string value of the corresponding line number.<see cref="string"/>.</param>
        /// <param name="lineNum">The line number<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool LineExactMatch(string expectedString, int lineNum)
        {
            this.Open();

            using (this.reader)
            {
                string line = string.Empty;
                int lineIndex = 1;
                while ((line = this.reader.ReadLine()) != null)
                {
                    if (lineIndex == lineNum)
                    {
                        return line == expectedString;
                    }

                    lineIndex++;
                }
            }

            return false;
        }

        /// <summary>
        /// This method appends a line to the end of the text file.
        /// </summary>
        /// <param name="contents">The string to be added to the end of the file.</param>
        public void AddLine(string contents)
        {
            // Close file first so we can just append to the file.
            this.Close();
            File.AppendAllText(this.FilePath, contents + Environment.NewLine);
        }

        /// <summary>
        /// Create a resulting error log file for the comparison.
        /// </summary>
        /// <param name="filePath">The file path to create the error log.<see cref="string"/>.</param>
        private void CreateErrorLog(string filePath)
        {
            if (this.log.Any())
            {
                File.WriteAllLines(filePath, this.log);
                this.log.Clear();
            }
        }
    }
}
