// <copyright file="Interactor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="Interactor" />.
    /// </summary>
    public class Interactor : TextFile
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
        /// Initializes a new instance of the <see cref="Interactor"/> class.
        /// </summary>
        /// <param name="filePath">The file path of the text file.</param>
        public Interactor(string filePath)
        {
            this.FilePath = filePath;
            this.Opened = false;
            this.reader = null;
            this.log = new List<string>();
        }

        /// <summary>
        /// This method opens the text file for reading and writing.
        /// </summary>
        /// <returns><code>true</code> if file was successfully opened.</returns>
        public override bool Open()
        {
            try
            {
                // Check if file exists. If it doesn't, check on desktop and downloads
                if (!File.Exists(this.FilePath))
                {
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//" + this.FilePath))
                    {
                        this.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//" + this.FilePath;
                    }
                    else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "..//Downloads/" + this.FilePath))
                    {
                        this.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "..//Downloads/" + this.FilePath;
                    }
                }

                this.reader = new StreamReader(this.FilePath);
                this.Opened = true;
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is FileNotFoundException || ex is DirectoryNotFoundException)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }

                throw ex;
            }

            return true;
        }

        /// <summary>
        /// This method closes the text file.
        /// </summary>
        /// <returns><code>true</code> if file was successfully closed.</returns>
        public override bool Close()
        {
            if (!this.Opened)
            {
            }

            try
            {
                this.reader.Close();
                this.reader.Dispose();
                this.Opened = false;
                Console.WriteLine("TextInteractor: file has been closed: " + this.FilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method compares the text file with another file and returns whether or not the contents of both files are the same.
        /// </summary>
        /// <param name="txtFile">a text file to be compared with.</param>
        /// <returns><code>true</code> if the contents of both files are the same.</returns>
        public override bool Compare(TextFile txtFile)
        {
            return this.Compare(txtFile, ignoreWhitespace: false, caseInsensitive: false);
        }

        /// <summary>
        /// This method compares the text file with another file and returns whether or not the contents of both files are the same.
        /// Once file comparison check is completed, the readings of both files are restarted and if there are any discrepancies a
        /// compare log file will be generated.
        /// </summary>
        /// <param name="txtFile">a text file to be compared with.</param>
        /// <param name="ignoreWhitespace">if. <code>true</code>, whitespace is ignored while comparing.</param>
        /// <param name="caseInsensitive">if. <code>true</code>, case sensitivity is ignored while comparing.</param>
        /// <returns><code>true</code> if the contents of both files are the same.</returns>
        public override bool Compare(TextFile txtFile, bool ignoreWhitespace = false, bool caseInsensitive = false)
        {
            bool areEqual = true;
            string errorMsg = string.Empty;

            if (!this.Opened)
            {
                this.Open();
            }

            if (!txtFile.Opened)
            {
                txtFile.Open();
            }

            TextPrintHelper.PrintCompareBegin(this, txtFile, ignoreWhitespace, caseInsensitive);

            // restart the readers of both files
            this.RestartReading();
            txtFile.RestartReading();

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
                    int diffIndex = TextPrintHelper.FirstDifferentChar(lineA, lineB);
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

            TextPrintHelper.PrintCompareEnd(this, areEqual, this.log);

            // restart the readers of both files again
            this.RestartReading();
            txtFile.RestartReading();

            // create compare log file if discrepancies exists
            string logPath;
            if ((logPath = this.CreateErrorLog()).Any())
            {
                Console.WriteLine("Compare log saved at " + logPath);
            }

            return areEqual;
        }

        /// <inheritdoc/>
        public override bool Compare(TextFile txtFile, int startingLine, int startingIndex, int endingLine, int endingIndex)
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
            string errorMsg = string.Empty;
            string expectedFileLine = string.Empty;
            string actualFileLine = string.Empty;

            int lineIndex = 1;

            while ((expectedFileLine = this.ReadLine()) != null && lineIndex <= endingIndex)
            {
                actualFileLine = txtFile.ReadLine();
                if (actualFileLine == null)
                {
                    Console.WriteLine("File length are not the same.");
                    return false;
                }

                expectedFileLine = expectedFileLine.Substring(startingIndex, endingIndex);
                actualFileLine = actualFileLine.Substring(startingIndex, endingIndex);

                if (expectedFileLine != actualFileLine)
                {
                    int diffIndex = TextPrintHelper.FirstDifferentChar(expectedFileLine, actualFileLine);
                    errorMsg = "line " + lineIndex + " is not equal in both files";
                    errorMsg += "\n   Expected File: " + expectedFileLine;
                    errorMsg += "\n   Actual File: " + actualFileLine;
                    errorMsg += "\n           " + string.Concat(Enumerable.Repeat(" ", diffIndex)) + "^";
                    this.log.Add(errorMsg);
                    areEqual = false;
                }

                lineIndex++;
            }

            TextPrintHelper.PrintCompareEnd(this, areEqual, this.log);

            // create compare log file if discrepancies exists
            string logPath;
            if ((logPath = this.CreateErrorLog()).Any())
            {
                Console.WriteLine("Compare log saved at " + logPath);
            }

            return areEqual;
        }

        /// <summary>
        /// The Find.
        /// </summary>
        /// <param name="expectedString">The expectedString<see cref="string"/>.</param>
        /// <returns><code>true</code> if expectedString is found<see cref="bool"/>.</returns>
        public bool Find(string expectedString)
        {
            if (!this.Opened)
            {
                this.Open();
            }

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
        /// <param name="expectedString">The expectedString<see cref="string"/>.</param>
        /// <returns>The count of the expectedString in file <see cref="int"/>.</returns>
        public int FindAndCount(string expectedString)
        {
            if (!this.Opened)
            {
                this.Open();
            }

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

        /// <inheritdoc/>
        public override bool Find(string expectedString, int lineNum)
        {
            if (!this.Opened)
            {
                this.Open();
            }

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

        /// <summary>
        /// The LineExactMatch.
        /// </summary>
        /// <param name="expectedString">The expectedString<see cref="string"/>.</param>
        /// <param name="lineNum">The lineNum<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool LineExactMatch(string expectedString, int lineNum)
        {
            if (!this.Opened)
            {
                this.Open();
            }

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
        /// This method reads a line of characters from the file, returns it as a string, and moves to the next line.
        /// </summary>
        /// <returns>A string read from the text file.</returns>
        public override string ReadLine()
        {
            if (!this.Opened)
            {
                this.Open();
            }

            return this.reader.EndOfStream ? null : this.reader.ReadLine();
        }

        /// <summary>
        /// This method restarts the reader back to the first line of the text file.
        /// </summary>
        public override void RestartReading()
        {
            if (!this.Opened)
            {
                this.Open();
            }

            this.reader.DiscardBufferedData();
            this.reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// This method returns whether or not the reader has reached the end of the file.
        /// </summary>
        /// <returns><code>true</code> if reader is at the end of the file.</returns>
        public override bool FinishedReading()
        {
            if (!this.Opened)
            {
                this.Open();
            }

            return this.reader.EndOfStream;
        }

        /// <summary>
        /// This method appends a line to the end of the text file.
        /// </summary>
        /// <param name="contents">The string to be added to the end of the file.</param>
        public void AddLine(string contents)
        {
            if (this.Opened)
            {
                this.Close();
            }

            File.AppendAllText(this.FilePath, contents + Environment.NewLine);
        }

        /// <inheritdoc/>
        public override bool Modify(int replaceType, string args)
        {
            const int REPLACEONCE = 0;
            const int REPLACEALL = 1;
            const int REPLACELINE = 2;
            const int REPLACEREGEX = 3;
            const string seperator = "];[";

            if (!this.Opened)
            {
                this.Open();
            }

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
                        Console.WriteLine("                Provided arguments are not in the correct format of stringToBeReplaced];[replacementString");
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
                        Console.WriteLine("                Provided arguments are not in the correct format of line1;line2;line3];[replacementString or lineRange1;lineRange2;lineRange3];[replacementString");
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
                        Console.WriteLine("                Provided arguments are not in the correct format of stringToBeReplaced];[replacementString");
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

        /// <summary>
        /// The CreateErrorLog.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string CreateErrorLog(string path = null)
        {
            if (this.log.Any())
            {
                string fileName = DateTime.Now.ToString("Run_dd-MM-yyyy_HH-mm-ss") + ".log";
                if (path == null)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    path += "\\" + fileName;
                }

                File.AppendAllLines(path, this.log);
                this.log.Clear();
                return path;
            }

            return string.Empty;
        }
    }
}
