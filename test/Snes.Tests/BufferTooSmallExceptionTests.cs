// <copyright file="BufferTooSmallExceptionTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using System;
using Xunit;

public class BufferTooSmallExceptionTests
{
    [Fact]
    public void Constructor_WithoutArguments_UsesBufferTooSmallMessage()
    {
        var exception = new BufferTooSmallException();

        Assert.Null(exception.ParamName);
        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithParamName_UsesBufferTooSmallMessage()
    {
        var exception = new BufferTooSmallException("buffer");

        Assert.Equal("buffer", exception.ParamName);
        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_PreservesArguments()
    {
        var innerException = new InvalidOperationException();
        var exception = new BufferTooSmallException("message", innerException);

        Assert.Equal("message", exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndParamName_PreservesArguments()
    {
        var exception = new BufferTooSmallException("message", "buffer");

        Assert.Contains("message", exception.Message);
        Assert.Equal("buffer", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithMessageParamNameAndInnerException_PreservesArguments()
    {
        var innerException = new InvalidOperationException();
        var exception = new BufferTooSmallException(
            "message",
            "buffer",
            innerException);

        Assert.Contains("message", exception.Message);
        Assert.Equal("buffer", exception.ParamName);
        Assert.Same(innerException, exception.InnerException);
    }
}
