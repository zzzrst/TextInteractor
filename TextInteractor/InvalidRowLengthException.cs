// <copyright file="InvalidRowLengthException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Exception for invalid row lengths.
    /// </summary>
    internal class InvalidRowLengthException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRowLengthException"/> class.
        /// </summary>
        public InvalidRowLengthException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRowLengthException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public InvalidRowLengthException(string message)
            : base(message)
        {
        }
    }
}
