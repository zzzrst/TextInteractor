// <copyright file="TextDriver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

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
            file.WriteLine("Employee name is [{#john#}], works for [{#ABC BANK#}],[{#Houston#}]");
            file.WriteLine("Bye NOW!");
            file.Close();
            Interactor testFileA = new Interactor(testFile);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // @"\[\{\#(.*?)\#\}\]
            testFileA.Modify(3, @"[a-zA-Z0-9]];[yo");

            watch.Stop();
            Console.WriteLine("\nThat took " + watch.ElapsedMilliseconds.ToString() + " milliseconds!");
        }
    }
}
