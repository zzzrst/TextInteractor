// <copyright file="CSVInteractor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="CSVInteractor" />.
    /// </summary>
    public class CSVInteractor : TextFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSVInteractor"/> class.
        /// </summary>
        /// <param name="filePath"> The filePath to find the CSV.</param>
        public CSVInteractor(string filePath)
        {
            this.FilePath = filePath;
            this.Opened = false;
            this.Header = null;
            this.Log = new List<string>();
        }

        /// <summary>
        /// Gets the Header
        /// List containing the header titles of the CSV file.
        /// </summary>
        public List<string> Header { get; private set; }

        /// <summary>
        /// Gets or sets the Reader
        /// Gets or sets streamReader to read the CSV file.
        /// </summary>
        private StreamReader Reader { get; set; }

        /// <summary>
        /// Gets or sets the Log.
        /// </summary>
        private List<string> Log { get; set; }

        /// <summary>
        /// This method opens the CSV file for reading and writing.
        /// </summary>
        /// <returns><code>true</code> if CSV file successfully opens.</returns>
        public override bool Open()
        {
            try
            {
                // open the file stream, ready the file for reading and writing
                this.Reader = new StreamReader(this.FilePath);

                // get the headers of the CSV file
                string line = this.Reader.ReadLine();
                this.Header = line.Split(',').ToList();

                // we are done reading
                Console.WriteLine("CSVInteractor: file has been opened: " + this.FilePath);
                this.Opened = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return this.Opened;
        }

        /// <summary>
        /// This method closes the CSV file for reading and writing.
        /// </summary>
        /// <returns><code>true</code> if CSV file successfully closes.</returns>
        public override bool Close()
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            try
            {
                this.Reader.Close();
                this.Header = null;
                this.Opened = false;
                Console.WriteLine("CSVInteractor: file has been closed: " + this.FilePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// This method compares the CSV file with another text file. Both files must be opened.
        /// </summary>
        /// <param name="txtFile">the other text file to be compared to.</param>
        /// <returns><code>true</code> if contents of both files are identical.</returns>
        public override bool Compare(TextFile txtFile)
        {
            // check if both files are opened
            if (!this.Opened || !txtFile.Opened)
            {
                throw new FileNotOpenedException("text files are not opened for reading and writing");
            }

            // print statements
            TextPrintHelper.PrintCompareBegin(this, txtFile, false, false);

            // kick off compare
            return this.InternalCompare(txtFile, ignoreWhitespace: false, caseInsensitive: false);
        }

        /// <summary>
        /// This method compares the CSV file with another text file. Both files must be opened.
        /// </summary>
        /// <param name="txtFile">the other text file to be compared to.</param>
        /// <param name="ignoreWhitespace">The ignoreWhitespace<see cref="bool"/>.</param>
        /// <param name="caseInsensitive">The caseInsensitive<see cref="bool"/>.</param>
        /// <returns><code>true</code> if contents of both files are identical.</returns>
        public override bool Compare(TextFile txtFile, bool ignoreWhitespace = false, bool caseInsensitive = false)
        {
            // check if both files are opened
            if (!this.Opened || !txtFile.Opened)
            {
                throw new FileNotOpenedException("text files are not opened for reading and writing");
            }

            TextPrintHelper.PrintCompareBegin(this, txtFile, ignoreWhitespace, caseInsensitive);

            // go to compare method depending on text file type and if ignore whitespace is enabled
            if (txtFile is CSVInteractor csvFile && ignoreWhitespace)
            {
                return this.InternalCompare(csvFile, caseInsensitive);
            }
            else
            {
                return this.InternalCompare(txtFile, ignoreWhitespace, caseInsensitive);
            }
        }

        /// <summary>
        /// This method reads a line of characters from the file and returns it as a string.
        /// </summary>
        /// <returns>A string read from the file.</returns>
        public override string ReadLine()
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            if (!this.Reader.EndOfStream)
            {
                return this.Reader.ReadLine();
            }

            return null;
        }

        /// <summary>
        /// This method reads and parses a row from the CSV file.
        /// </summary>
        /// <returns>A string array of the line's contents.</returns>
        public string[] ReadAndParseLine()
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            string line;
            if ((line = this.ReadLine()) != null)
            {
                return line.Split(',');
            }

            return null;
        }

        /// <summary>
        /// This method reads and parses a row from the CSV file.
        /// </summary>
        /// <param name="trim">If true, removes surrounding whitespace from the parsed values.</param>
        /// <returns>A string array of the line's contents.</returns>
        public string[] ReadAndParseLine(bool trim)
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            string[] values = this.ReadAndParseLine();
            if (values != null && trim)
            {
                values = values.Select(value => value.Trim()).ToArray();
            }

            return values;
        }

        /// <summary>
        /// This method resets the reading of the CSV file back to the header.
        /// </summary>
        public override void RestartReading()
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            this.Reader.DiscardBufferedData();
            this.Reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        /// <summary>
        /// This method returns whether or not the interactor is at the end of the file.
        /// </summary>
        /// <returns><code>true</code> if the interactor is at the end of the CSV file.</returns>
        public override bool FinishedReading()
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException("file must be opened");
            }

            return this.Reader.EndOfStream;
        }

        /// <summary>
        /// This method adds a row of values to the CSV file, in the order they were inputted in the list.
        /// </summary>
        /// <param name="values">A list of values for the row to be added.</param>
        public void AddRow(List<string> values)
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            // check if values has valid length, throw exception if not
            if (values.Count > this.Header.Count)
            {
                throw new InvalidRowLengthException("Number of values must be less than titles in CSV header");
            }

            // add empty values if length of values is less
            int numLoops = this.Header.Count - values.Count;
            for (int i = 0; i < numLoops; ++i)
            {
                values.Add(string.Empty);
            }

            // create new line, append to CSV file
            string newLine = string.Join(",", values);
            File.Copy(this.FilePath, this.FilePath + ".tmp");
            File.AppendAllText(this.FilePath + ".tmp", newLine + Environment.NewLine);
            this.Close();
            File.Replace(this.FilePath + ".tmp", this.FilePath, this.FilePath + ".tmp2");
            File.Delete(this.FilePath + ".tmp2");
            this.Open();
            Console.WriteLine("CSVInteractor: row has been added to " + this.FilePath);
        }

        /// <summary>
        /// This method adds a row of values to the CSV file, in the order they were inputted in the array.
        /// </summary>
        /// <param name="values">An array of values for the row to be added.</param>
        public void AddRow(params string[] values)
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            // convert array to list
            this.AddRow(values.ToList());
        }

        /// <summary>
        /// This method adds a row of values to the CSV file, where the values are arranged by the
        /// order of the titles they were mapped to.
        /// </summary>
        /// <param name="values">A dictionary of column titles mapping to their respective values.</param>
        public void AddRow(Dictionary<string, string> values)
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            // check if all the inputted titles exist in the CSV's header
            List<string> inputTitles = values.Keys.ToList();
            if (inputTitles.Except(this.Header).Any())
            {
                throw new InvalidHeaderTitleException("header titles are not valid");
            }

            // create a new list, with the values arranged in the order of the header
            List<string> newValues = new List<string>();
            foreach (string title in this.Header)
            {
                if (values.ContainsKey(title))
                {
                    newValues.Add(values[title]);
                }
                else
                {
                    newValues.Add(string.Empty);
                }
            }

            // pass in rearranged list
            this.AddRow(newValues);
        }

        /// <summary>
        /// This method adds a title and column of values to the CSV file.
        /// </summary>
        /// <param name="title">The new column title.</param>
        /// <param name="values">A list of values for the column to be added.</param>
        public void AddColumn(string title, List<string> values)
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            if (this.Header.Contains(title))
            {
                throw new InvalidHeaderTitleException("column title already exists in header");
            }

            File.Copy(this.FilePath, this.FilePath + ".tmp");
            try
            {
                // check if the number of values is less than or equal to rows
                List<string> allLines = File.ReadAllLines(this.FilePath + ".tmp").ToList();
                if (values.Count > allLines.Count - 1)
                {
                    throw new InvalidColumnLengthException("number of values must be less than or equal to rows in CSV file");
                }

                // add new column title to header row
                allLines[0] += "," + title;
                int i = 1;

                // add new column value for each row
                allLines.Skip(1).ToList().ForEach(line =>
                {
                    allLines[i] += ",";
                    if (i <= values.Count)
                    {
                        // -1 for header
                        allLines[i] += values[i - 1];
                    }

                    ++i;
                });

                // write the new content
                File.WriteAllLines(this.FilePath + ".tmp", allLines);
                this.Close();
                File.Replace(this.FilePath + ".tmp", this.FilePath, this.FilePath + ".tmp2");
                File.Delete(this.FilePath + ".tmp2");
                this.Open();
                Console.WriteLine("CSVInteractor: column has been added to " + this.FilePath);
            }
            catch (InvalidColumnLengthException ex)
            {
                File.Delete(this.FilePath + ".tmp");
                throw ex;
            }
        }

        /// <summary>
        /// This method adds a title and column of values to the CSV file.
        /// </summary>
        /// <param name="title">The new column title.</param>
        /// <param name="values">An array of values for the column to be added.</param>
        public void AddColumn(string title, params string[] values)
        {
            if (!this.Opened)
            {
                throw new FileNotOpenedException(FileNotOpenedException.ErrorMsg);
            }

            this.AddColumn(title, values.ToList());
        }

        /// <inheritdoc/>
        public override bool Modify(int replaceType, string args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Compare.
        /// </summary>
        /// <param name="txtFile">The txtFile<see cref="TextFile"/>.</param>
        /// <param name="startingLine">The startingLine<see cref="int"/>.</param>
        /// <param name="startingIndex">The startingIndex<see cref="int"/>.</param>
        /// <param name="endingLine">The endingLine<see cref="int"/>.</param>
        /// <param name="endingIndex">The endingIndex<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Compare(TextFile txtFile, int startingLine, int startingIndex, int endingLine, int endingIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Find.
        /// </summary>
        /// <param name="expectedString">The expectedString<see cref="string"/>.</param>
        /// <param name="line">The line<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Find(string expectedString, int line)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The InternalCompare.
        /// </summary>
        /// <param name="csvFile">The csvFile<see cref="CSVInteractor"/>.</param>
        /// <param name="caseInsensitive">The caseInsensitive<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool InternalCompare(CSVInteractor csvFile, bool caseInsensitive)
        {
            bool areEqual = true;
            string errorMsg = string.Empty;
            int lineNumber = 1;

            // restart reading of both files
            this.RestartReading();
            csvFile.RestartReading();

            // loop through first file, compare with second file
            string[] valuesA;
            bool unequalLineCount = false;
            Console.WriteLine("CSVInteractor: comparing lines... this may take a while for big files");
            while ((valuesA = this.ReadAndParseLine(trim: true)) != null)
            {
                string[] valuesB = csvFile.ReadAndParseLine(trim: true);

                // check if second file still has lines to be read
                if (valuesB == null)
                {
                    errorMsg = "File B has less lines than File A";
                    errorMsg += "\n   File B: " + (lineNumber - 1) + " lines";
                    areEqual = false;
                    unequalLineCount = true;
                    break;
                }

                // if case insensitivity check is enabled
                if (caseInsensitive)
                {
                    valuesA = valuesA.Select(value => value.ToUpper()).ToArray();
                    valuesB = valuesB.Select(value => value.ToUpper()).ToArray();
                }

                // check if valuesA is equal to valuesB
                if (!valuesA.SequenceEqual(valuesB))
                {
                    string lineA = string.Join(",", valuesA);
                    string lineB = string.Join(",", valuesB);
                    int diffIndex = TextPrintHelper.FirstDifferentChar(lineA, lineB);
                    errorMsg = "line " + lineNumber + " is not equal in both files";
                    errorMsg += "\n   File A: " + lineA;
                    errorMsg += "\n   File B: " + lineB;
                    errorMsg += "\n           " + string.Concat(Enumerable.Repeat(" ", diffIndex)) + "^";
                    this.Log.Add(errorMsg);
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
                this.Log.Add(errorMsg);
            }

            // check if second file has finished reading
            if (!csvFile.FinishedReading())
            {
                errorMsg = "File A has less lines than File B";
                errorMsg += "\n   File A: " + lineNumber + " lines";
                while (csvFile.ReadLine() != null)
                {
                    ++lineNumber;
                }

                errorMsg += "\n   File B: " + lineNumber + " lines";
                this.Log.Add(errorMsg);
                areEqual = false;
            }

            TextPrintHelper.PrintCompareEnd(this, areEqual, this.Log);

            // restart reading of both files
            this.RestartReading();
            csvFile.RestartReading();

            // create compare log file if discrepancies exists
            string logPath;
            if ((logPath = this.InternalCreateErrorLog()).Any())
            {
                Console.WriteLine("Compare log saved at " + logPath);
            }

            return areEqual;
        }

        /// <summary>
        /// The InternalCompare.
        /// </summary>
        /// <param name="txtFile">The txtFile<see cref="TextFile"/>.</param>
        /// <param name="ignoreWhitespace">The ignoreWhitespace<see cref="bool"/>.</param>
        /// <param name="caseInsensitive">The caseInsensitive<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool InternalCompare(TextFile txtFile, bool ignoreWhitespace, bool caseInsensitive)
        {
            bool areEqual = true;
            string errorMsg = string.Empty;
            int lineNumber = 1;

            // restart the readers of both files
            this.RestartReading();
            txtFile.RestartReading();

            // read until the end of file A
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
                    lineA = lineA.Trim();
                    lineB = lineB.Trim();
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
                    this.Log.Add(errorMsg);
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
                this.Log.Add(errorMsg);
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
                this.Log.Add(errorMsg);
                areEqual = false;
            }

            TextPrintHelper.PrintCompareEnd(this, areEqual, this.Log);

            // restart the readers of both files again
            this.RestartReading();
            txtFile.RestartReading();

            // create compare log file if discrepancies exists
            string logPath;
            if ((logPath = this.InternalCreateErrorLog()).Any())
            {
                Console.WriteLine("Compare log saved at " + logPath);
            }

            return areEqual;
        }

        /// <summary>
        /// The CreateErrorLog.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string InternalCreateErrorLog(string path = null)
        {
            if (this.Log.Any())
            {
                string fileName = DateTime.Now.ToString("Run_dd-MM-yyyy_HH-mm-ss") + ".log";
                if (path == null)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    path += "\\" + fileName;
                }

                File.AppendAllLines(path, this.Log);
                this.Log.Clear();
                return path;
            }

            return string.Empty;
        }
    }
}
