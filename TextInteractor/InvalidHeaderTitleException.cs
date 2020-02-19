// <copyright file="InvalidHeaderTitleException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TextInteractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Exception for invalid header titles.
    /// </summary>
    internal class InvalidHeaderTitleException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderTitleException"/> class.
        /// </summary>
        public InvalidHeaderTitleException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderTitleException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public InvalidHeaderTitleException(string message)
            : base(message)
        {
        }
    }
}
