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
                this.Logger.LogError(ex.ToString());
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
                    this.Logger.LogInformation("TextInteractor: file has been closed: " + this.FilePath);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex.ToString());
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

            using (this.reader)
            {
                string line = string.Empty;
                int lineIndex = 1;

                while ((line = this.reader.ReadLine()) != null && lineIndex <= lineNum)
                {
                    if (lineIndex == lineNum)
                    {
                        return line.Contains(expectedString);
                    }

                    lineIndex++;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool Modify(int replaceType, string args)
        {
            const int REPLACEONCE = 0;
            const int REPLACEALL = 1;
            const int REPLACELINE = 2;
            const int REPLACEREGEX = 3;
            const string seperator = "];[";

            this.Open();

            bool replacedOnce = false;
            string toBeReplaced = string.Empty;
            string replacementString = string.Empty;
            List<int> lines = new List<int>();

            // argument parsing
            switch (replaceType)
            {
                case REPLACEONCE:
                case REPLACEALL:

                    try
                    {
                        // argument will come in format stringToBeReplaced];[replacementString
                        toBeReplaced = args.Substring(0, args.IndexOf(seperator));
                        replacementString = args.Substring(args.IndexOf(seperator) + 3);
                    }
                    catch
                    {
                        this.Logger.LogError("                Provided arguments are not in the correct format of stringToBeReplaced];[replacementString");
                        return false;
                    }

                    break;
                case REPLACELINE:

                    // argument will be in the form of line;line;line];[replacementString or line-range;line-range;];[replacementString
                    try
                    {
                        string lineArgs = args.Substring(0, args.IndexOf(seperator));
                        replacementString = args.Substring(args.IndexOf(seperator) + 3);

                        List<string> lineArgsSeperated = lineArgs.Split(';').ToList();
                        foreach (string lineArg in lineArgsSeperated)
                        {
                            if (lineArg.Contains("-"))
                            {
                                int start = int.Parse(lineArg.Substring(0, args.IndexOf("-")));
                                int end = int.Parse(lineArg.Substring(args.IndexOf("-") + 1));

                                while (start <= end)
                                {
                                    lines.Add(start);
                                    start++;
                                }
                            }
                            else
                            {
                                lines.Add(int.Parse(lineArg));
                            }
                        }
                    }
                    catch
                    {
                        this.Logger.LogError("                Provided arguments are not in the correct format of line1;line2;line3];[replacementString or lineRange1;lineRange2;lineRange3];[replacementString");
                        return false;
                    }

                    break;
                case REPLACEREGEX:

                    try
                    {
                        // argument will come in format stringToBeReplaced];[replacementString
                        toBeReplaced = args.Substring(0, args.IndexOf(seperator));
                        replacementString = args.Substring(args.IndexOf(seperator) + 3);
                    }
                    catch
                    {
                        this.Logger.LogError("                Provided arguments are not in the correct format of stringToBeReplaced];[replacementString");
                        return false;
                    }

                    break;
            }

            // use the Stream
            using (this.reader)
            {
                using (var tempFile = new StreamWriter(this.FilePath + ".tmp"))
                {
                    string line;
                    int lineIndex = 1;
                    while ((line = this.reader.ReadLine()) != null)
                    {
                        switch (replaceType)
                        {
                            case REPLACEONCE:
                                if (!replacedOnce)
                                {
                                    if (line.Contains(toBeReplaced))
                                    {
                                        line = line.Replace(toBeReplaced, replacementString);
                                        replacedOnce = true;
                                    }
                                }

                                break;

                            case REPLACEALL:
                                line = line.Replace(toBeReplaced, replacementString);
                                break;
                            case REPLACELINE:
                                if (lines.Contains(lineIndex))
                                {
                                    line = replacementString;
                                }

                                break;
                            case REPLACEREGEX:
                                line = Regex.Replace(line, toBeReplaced, replacementString);
                                break;
                        }

                        tempFile.WriteLine(line);
                        lineIndex++;
                    }
                }

                this.reader.Close();
                File.Delete(this.FilePath);
                File.Move(this.FilePath + ".tmp", this.FilePath);
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Compare(TextFile txtFile, string resultFilePath, bool ignoreWhitespace = false, bool caseInsensitive = false)
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

                ++lineNumber;
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
                this.Logger.LogInformation("Compare log saved at " + resultFilePath);
            }

            return areEqual;
        }

        /// <inheritdoc/>
        public override bool Compare(TextFile txtFile, string resultFilePath, int startingLine, int startingIndex, int endingLine, int endingIndex, bool ignoreWhitespace = false, bool caseInsensitive = false)
        {
            if (!this.Opened)
            {
                this.Open();
            }

            if (!txtFile.Opened)
            {
                txtFile.Open();
            }

            bool areEqual = true;
            int lineIndex = 1;
            string expectedFileLine;
            while ((expectedFileLine = this.ReadLine()) != null && lineIndex <= endingIndex)
            {
                string actualFileLine = txtFile.ReadLine();
                if (actualFileLine == null)
                {
                    this.Logger.LogInformation("File length are not the same.");
                    return false;
                }

                expectedFileLine = expectedFileLine.Substring(startingIndex, endingIndex);
                actualFileLine = actualFileLine.Substring(startingIndex, endingIndex);

                if (expectedFileLine != actualFileLine)
                {
                    int diffIndex = TextFileLogHelper.FirstDifferentChar(expectedFileLine, actualFileLine);
                    string errorMsg = $"line {lineIndex} is not equal in both files \n";
                    errorMsg += $"   Expected File: {expectedFileLine}\n";
                    errorMsg += $"   Actual File: {actualFileLine}\n";
                    errorMsg += "           " + string.Concat(Enumerable.Repeat(" ", diffIndex)) + "^";
                    this.log.Add(errorMsg);
                    areEqual = false;
                }

                lineIndex++;
            }

            TextFileLogHelper.LogComparisonEnd(this, areEqual, this.log);

            // create compare log file if discrepancies exists
            if (this.log.Any())
            {
                this.CreateErrorLog(resultFilePath);
                this.Logger.LogInformation("Compare log saved at " + resultFilePath);
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
