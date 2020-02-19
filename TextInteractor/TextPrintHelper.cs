// <copyright file="TextPrintHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="TextPrintHelper" />.
    /// </summary>
    internal class TextPrintHelper
    {
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
        /// The PrintBigTitle.
        /// </summary>
        /// <param name="title">The title<see cref="string"/>.</param>
        public static void PrintBigTitle(string title)
        {
            Console.WriteLine();
            Console.WriteLine(string.Format("****** {0} ******", title));
            Console.WriteLine();
        }

        /// <summary>
        /// The PrintCompareBegin.
        /// </summary>
        /// <param name="fileA">The fileA<see cref="TextFile"/>.</param>
        /// <param name="fileB">The fileB<see cref="TextFile"/>.</param>
        /// <param name="ignoreWhitespace">The ignoreWhitespace<see cref="bool"/>.</param>
        /// <param name="caseInsensitive">The caseInsensitive<see cref="bool"/>.</param>
        public static void PrintCompareBegin(TextFile fileA, TextFile fileB, bool ignoreWhitespace, bool caseInsensitive)
        {
            PrintBigTitle("BEGINNING FILE COMPARISION");
            Console.WriteLine(fileA.GetType().Name + ": files to compare");
            Console.WriteLine("   File A: " + fileA.FilePath);
            Console.WriteLine("   File B: " + fileB.FilePath);
            Console.WriteLine();
            if (ignoreWhitespace)
            {
                Console.WriteLine(fileA.GetType().Name + ": ignore whitespace comparison enabled");
            }

            if (caseInsensitive)
            {
                Console.WriteLine(fileA.GetType().Name + ": case insensitivity comparison enabled");
            }

            if (ignoreWhitespace || caseInsensitive)
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// The PrintCompareEnd.
        /// </summary>
        /// <param name="fileA">The fileA<see cref="TextFile"/>.</param>
        /// <param name="passed">The passed<see cref="bool"/>.</param>
        /// <param name="reasons">The reasons<see cref="T:string[]"/>.</param>
        public static void PrintCompareEnd(TextFile fileA, bool passed, params string[] reasons)
        {
            if (reasons.Any())
            {
                Console.Write("\n" + fileA.GetType().Name + ": ");
            }

            foreach (string reason in reasons)
            {
                Console.WriteLine(reason);
            }

            PrintBigTitle(passed ? "FILE COMPARISION STATUS PASSED" : "FILE COMPARISION STATUS FAILED");
        }

        /// <summary>
        /// The PrintCompareEnd.
        /// </summary>
        /// <param name="fileA">The fileA<see cref="TextFile"/>.</param>
        /// <param name="passed">The passed<see cref="bool"/>.</param>
        /// <param name="reasons">The reasons<see cref="T:List{string}"/>.</param>
        public static void PrintCompareEnd(TextFile fileA, bool passed, List<string> reasons)
        {
            PrintCompareEnd(fileA, passed, reasons.ToArray());
        }

        /// <summary>
        /// The PrintEnumerable.
        /// </summary>
        /// <param name="list">The list<see cref="T:IEnumerable{string}"/>.</param>
        public static void PrintEnumerable(IEnumerable<string> list)
        {
            string contents = string.Empty;
            if (list != null)
            {
                contents = string.Join(", ", list);
            }

            Console.WriteLine("[" + contents + "]");
        }
    }
}
