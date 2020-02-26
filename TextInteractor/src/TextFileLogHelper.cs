// <copyright file="TextFileLogHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="TextFileLogHelper" />.
    /// </summary>
    internal static class TextFileLogHelper
    {
        /// <summary>
        /// Gets or sets the logger to be used throughout this class.
        /// </summary>
        public static ILogger Logger { get; set; }

        /// <summary>
        /// The FirstDifferentChar.
        /// </summary>
        /// <param name="strA">The strA<see cref="string"/>.</param>
        /// <param name="strB">The strB<see cref="string"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int FirstDifferentChar(string strA, string strB)
        {
            return strA.Zip(strB, (c1, c2) => c1 == c2).TakeWhile(b => b).Count();
        }

        /// <summary>
        /// Creates a big title.
        /// </summary>
        /// <param name="title">The title<see cref="string"/>.</param>
        public static void LogBigTitle(string title)
        {
            Logger.LogInformation(string.Empty);
            Logger.LogInformation($"****** {title} ******");
            Logger.LogInformation(string.Empty);
        }

        /// <summary>
        /// The LogComparisonBeginning.
        /// </summary>
        /// <param name="fileA">The first file to compare.<see cref="TextFile"/>.</param>
        /// <param name="fileB">The second file to compare.<see cref="TextFile"/>.</param>
        /// <param name="ignoreWhitespace">The flag set for comparison if whitespace is ignored or not.<see cref="bool"/>.</param>
        /// <param name="caseInsensitive">The flas set for case insensitive comparison.<see cref="bool"/>.</param>
        public static void LogComparisonBeginning(TextFile fileA, TextFile fileB, bool ignoreWhitespace, bool caseInsensitive)
        {
            LogBigTitle("BEGIN FILE COMPARISON");

            string comparisonType = fileA.GetType().Name;

            Logger.LogInformation($"{comparisonType}: files to compare");
            Logger.LogInformation($"   File A: {fileA.FilePath}");
            Logger.LogInformation($"   File B: {fileB.FilePath}");
            Logger.LogInformation(string.Empty);

            if (ignoreWhitespace)
            {
                Logger.LogInformation($"{comparisonType}: ignore whitespace comparison enabled");
            }

            if (caseInsensitive)
            {
                Logger.LogInformation($"{comparisonType}: case insensitivity comparison enabled");
            }

            if (ignoreWhitespace || caseInsensitive)
            {
                Logger.LogInformation(string.Empty);
            }
        }

        /// <summary>
        /// Helper method to log when the comparison of files have ended.
        /// </summary>
        /// <param name="fileA">The first file to compare.<see cref="TextFile"/>.</param>
        /// <param name="passed">Boolean result if the file comparison.<see cref="bool"/>.</param>
        /// <param name="reasons">The reasons provided for the result.<see cref="T:string[]"/>.</param>
        public static void LogComparisonEnd(TextFile fileA, bool passed, params string[] reasons)
        {
            if (reasons.Any())
            {
                Logger.LogInformation(string.Empty);
                Logger.LogInformation($"{fileA.GetType().Name}: ");
            }

            foreach (string reason in reasons)
            {
                Logger.LogInformation(reason);
            }

            LogBigTitle($"FILE COMPARISION STATUS: {(passed ? "Passed" : "Failure")} ");
        }

        /// <summary>
        /// The LogComparisonEnd.
        /// </summary>
        /// <param name="fileA">The first file to compare.<see cref="TextFile"/>.</param>
        /// <param name="passed">Boolean result if the file comparison.<see cref="bool"/>.</param>
        /// <param name="reasons">The reasons provided for the result.<see cref="T:List{string}"/>.</param>
        public static void LogComparisonEnd(TextFile fileA, bool passed, List<string> reasons)
        {
            LogComparisonEnd(fileA, passed, reasons.ToArray());
        }

        /// <summary>
        /// Helper method to log an IEnumarable string in one line.
        /// </summary>
        /// <param name="list">The list<see cref="T:IEnumerable{string}"/>.</param>
        public static void LogEnumerableAsOneLine(IEnumerable<string> list)
        {
            string contents = string.Empty;
            if (list != null)
            {
                contents = string.Join(", ", list);
            }

            Logger.LogInformation("[" + contents + "]");
        }
    }
}
