// <copyright file="TextPreprocessorTests.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;

    public class TextPreprocessorTests
    {
        [Fact]
        public void NullStringThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TextPreprocessor(null);
            });
        }

        [Fact]
        public void EmptyStringRemainsEmpty()
        {
            var textPreprocessor = new TextPreprocessor(String.Empty);

            Assert.Equal(String.Empty, textPreprocessor.ToString());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("\n", "\n")]
        [InlineData("\n\n", "\n\n")]
        [InlineData("Foo", "Foo\n")]
        [InlineData("Foo\n", "Foo\n")]
        [InlineData("Foo\n\nBar", "Foo\n\nBar\n")]
        public void AddTrailingNewLine(
            string inputText,
            string expected)
        {
            var textPreprocessor = new TextPreprocessor(
                inputText,
                addTrailingNewLine: true);

            Assert.Equal(expected, textPreprocessor.ToString());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("\n", "")]
        [InlineData("\n\n", "")]
        [InlineData("Foo", "Foo")]
        [InlineData("Foo\n", "Foo")]
        [InlineData("Foo\n\nBar", "Foo\n\nBar")]
        public void TrimTrailingNewLine(
            string inputText,
            string expected)
        {
            var textPreprocessor = new TextPreprocessor(
                inputText,
                trimTrailingNewLine: true);

            Assert.Equal(expected, textPreprocessor.ToString());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("\n", "\n")]
        [InlineData("\n\n", "\n")]
        [InlineData("Foo", "Foo\n")]
        [InlineData("Foo\n", "Foo\n")]
        [InlineData("Foo\n\nBar", "Foo\n\nBar\n")]
        public void AddAndTrimTrailingNewLine(
            string inputText,
            string expected)
        {
            var textPreprocessor = new TextPreprocessor(
                inputText,
                trimTrailingNewLine: true,
                addTrailingNewLine: true);

            Assert.Equal(expected, textPreprocessor.ToString());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("\r", "\n")]
        [InlineData("\r\n", "\n")]
        [InlineData("\rA", "\nA")]
        [InlineData("\r\r\n\r", "\n\n\n")]
        [InlineData("\n\r", "\n\n")]
        [InlineData("\n\r\n", "\n\n")]
        [InlineData("\r\n\r\n\r\n", "\n\n\n")]
        [InlineData("\rA\nB\r\nC\r\r\nD\n\rE\r\n", "\nA\nB\nC\n\nD\n\nE\n")]
        public void ConvertNewLines(
            string inputText,
            string expected)
        {
            var textPreprocessor = new TextPreprocessor(inputText);

            Assert.Equal(expected, textPreprocessor.ToString());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("?", "?")]
        [InlineData("?a", "?a")]
        [InlineData("??a", "??a")]
        [InlineData("??", "??")]
        [InlineData("??\r\n", "??\n")]
        [InlineData("?\r\n", "?\n")]
        [InlineData("???", "???")]
        [InlineData("???=", "?#")]
        [InlineData("??=", "#")]
        [InlineData("??=define", "#define")]
        [InlineData("??(??-0x100??)", "[~0x100]")]
        [InlineData("A??!B", "A|B")]
        [InlineData("A??'B", "A^B")]
        [InlineData("??< foo ??>", "{ foo }")]
        [InlineData("??\r??=?\r\n??(?\r??)", "??\n#?\n[?\n]")]
        public void ResolveTrigraphs(
            string inputText,
            string expected)
        {
            var textPreprocessor = new TextPreprocessor(
                inputText,
                resolveTrigraphs: true);

            Assert.Equal(expected, textPreprocessor.ToString());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("\u0080", "\\u0080")]
        [InlineData("F\uFFFFF", "F\\uFFFFF")]
        public void ForceAscii(
            string inputText,
            string expected)
        {
            var textPreprocessor = new TextPreprocessor(
                inputText,
                forceAscii: true);

            Assert.Equal(expected, textPreprocessor.ToString());
        }
    }
}
