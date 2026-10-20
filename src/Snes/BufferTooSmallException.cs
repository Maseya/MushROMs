// <copyright file="BufferTooSmallException.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;
using Maseya.Snes.Properties;

/// <summary>
/// The exception thrown when a buffer is too small for an operation.
/// </summary>
public class BufferTooSmallException : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BufferTooSmallException"/>
    /// class with the standard buffer-too-small message.
    /// </summary>
    public BufferTooSmallException()
        : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferTooSmallException"/>
    /// class with the standard message and the name of the parameter that
    /// contains the buffer.
    /// </summary>
    /// <param name="paramName">
    /// The name of the parameter that contains the undersized buffer.
    /// </param>
    public BufferTooSmallException(string? paramName)
        : base(Resources.BufferTooSmall, paramName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferTooSmallException"/>
    /// class with a specified message and inner exception.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    /// <param name="innerException">
    /// The exception that caused the current exception.
    /// </param>
    public BufferTooSmallException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferTooSmallException"/>
    /// class with a specified message and parameter name.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    /// <param name="paramName">
    /// The name of the parameter that caused the exception.
    /// </param>
    public BufferTooSmallException(string? message, string? paramName)
        : base(message, paramName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferTooSmallException"/>
    /// class with a specified message, parameter name, and inner exception.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    /// <param name="paramName">
    /// The name of the parameter that caused the exception.
    /// </param>
    /// <param name="innerException">
    /// The exception that caused the current exception.
    /// </param>
    public BufferTooSmallException(
        string? message,
        string? paramName,
        Exception? innerException)
        : base(message, paramName, innerException)
    {
    }
}
