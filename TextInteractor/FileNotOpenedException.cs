// <copyright file="FileNotOpenedException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;

    /// <summary>
    /// Exception for unopened files.
    /// </summary>
    public class FileNotOpenedException : Exception
    {
        /// <summary>
        /// Defines the ErrorMsg.
        /// </summary>
        public const string ErrorMsg = "text file is not opened for reading and writing";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileNotOpenedException"/> class.
        /// </summary>
        public FileNotOpenedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileNotOpenedException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public FileNotOpenedException(string message)
            : base(message)
        {
        }
    }
}
