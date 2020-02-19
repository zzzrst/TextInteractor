// <copyright file="InvalidColumnLengthException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Exception for invalid column lengths.
    /// </summary>
    internal class InvalidColumnLengthException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidColumnLengthException"/> class.
        /// </summary>
        public InvalidColumnLengthException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidColumnLengthException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public InvalidColumnLengthException(string message)
            : base(message)
        {
        }
    }
}
