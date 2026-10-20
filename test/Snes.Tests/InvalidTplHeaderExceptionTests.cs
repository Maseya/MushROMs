// <copyright file="InvalidTplHeaderExceptionTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using System;
using Xunit;

public class InvalidTplHeaderExceptionTests
{
    [Fact]
    public void Constructor_WithoutArguments_UsesInvalidTplHeaderMessage()
    {
        var exception = new InvalidTplHeaderException();

        Assert.Null(exception.ParamName);
        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithParamName_UsesInvalidTplHeaderMessage()
    {
        var exception = new InvalidTplHeaderException("bytes");

        Assert.Equal("bytes", exception.ParamName);
        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_PreservesArguments()
    {
        var innerException = new InvalidOperationException();
        var exception = new InvalidTplHeaderException("message", innerException);

        Assert.Equal("message", exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndParamName_PreservesArguments()
    {
        var exception = new InvalidTplHeaderException("message", "bytes");

        Assert.Contains("message", exception.Message);
        Assert.Equal("bytes", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithMessageParamNameAndInnerException_PreservesArguments()
    {
        var innerException = new InvalidOperationException();
        var exception = new InvalidTplHeaderException(
            "message",
            "bytes",
            innerException);

        Assert.Contains("message", exception.Message);
        Assert.Equal("bytes", exception.ParamName);
        Assert.Same(innerException, exception.InnerException);
    }
}
