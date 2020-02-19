// <copyright file="TextDriver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.IO;
    using System.Reflection;

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
            string testFile = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\text.txt";
            StreamWriter file = new StreamWriter(testFile);
            file.WriteLine("Hello World!");
            file.WriteLine("Time is now 12:34!");
            file.WriteLine("Bye Now!");
            file.Close();
            TextInteractor testFileA = new TextInteractor(testFile);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            testFileA.Modify(2, "1-1;3-5];[YAY");

            watch.Stop();
            Console.WriteLine("\nThat took " + watch.ElapsedMilliseconds.ToString() + " milliseconds!");
        }
    }
}
