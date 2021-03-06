﻿// MIT License
//
// Copyright (c) 2020 Hisham Maudarbocus
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Runtime.Serialization;

namespace LiteWare.Configuration.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a configuration key was not found.
    /// </summary>
    [Serializable]
    public class ConfigurationKeyNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationKeyNotFoundException"/> class.
        /// </summary>
        public ConfigurationKeyNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationKeyNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        public ConfigurationKeyNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationKeyNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ConfigurationKeyNotFoundException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationKeyNotFoundException" /> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        protected ConfigurationKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}