// <copyright file="TextDriver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;

    /// <summary>
    /// Defines the <see cref="TextDriver" />.
    /// </summary>
    internal class TextDriver
    {
        /// <summary>
        /// Main method used for testing.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        internal static void Main(string[] args)
        {
            string testFile = "test.txt";
            TextInteractor testFileA = new TextInteractor(testFile);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            testFileA.Modify(2, "1-1;3-5];[YAY");

            watch.Stop();
            Console.WriteLine("\nThat took " + watch.ElapsedMilliseconds.ToString() + " milliseconds!");
        }
    }
}
