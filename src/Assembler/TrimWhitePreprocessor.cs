// <copyright file="TrimWhitePreprocessor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public class TrimWhitePreprocessor : IEnumerable<TextUnit>
    {
        public TrimWhitePreprocessor(
            IEnumerable<char> text,
            bool allowSplice = false,
            string path = null,
            int file = -1)
        {
            Text = new TextPreprocessor(
                text,
                resolveTrigraphs: true,
                forceAscii: false,
                addTrailingNewLine: true,
                trimTrailingNewLine: true,
                file: file);

            Path = path;
        }

        public string Path
        {
            get;
        }

        public bool AllowSplice
        {
            get;
            set;
        }

        private TextPreprocessor Text
        {
            get;
        }

        public IEnumerator<TextUnit> GetEnumerator()
        {
            using (var builder = new Builder(this))
            using (var en = builder.GetEnumerator())
            {
                if (!en.MoveNext())
                {
                    var previous = en.Current;
                    for (; en.MoveNext(); previous = en.Current)
                    {
                        // Do not add trailing blank space at end of line.
                        if (previous != ' ' || en.Current != '\n')
                        {
                            yield return previous;
                        }
                    }

                    yield return previous;
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var x in this)
            {
                sb.Append(x);
            }

            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class Builder : IEnumerable<TextUnit>, IDisposable
        {
            public Builder(TrimWhitePreprocessor owner)
            {
                Owner = owner
                    ?? throw new ArgumentNullException(nameof(owner));

                Special = new Dictionary<char, Func<IEnumerable<TextUnit>>>()
                {
                    { ' ', TrimWhiteSpace },
                    { '\t', TrimWhiteSpace },
                    { ';', TrimLineComment },
                    { '/', TrimBlockComment },
                    { '"', AppendQuoteText },
                    { '\'', AppendQuoteText },
                };

                BaseEnumerator = Owner.GetEnumerator();
            }

            private TrimWhitePreprocessor Owner
            {
                get;
            }

            private IEnumerator<TextUnit> BaseEnumerator
            {
                get;
            }

            private Dictionary<char, Func<IEnumerable<TextUnit>>> Special
            {
                get;
            }

            private TextUnit Previous
            {
                get;
                set;
            }

            private TextUnit Current
            {
                get
                {
                    return BaseEnumerator.Current;
                }
            }

            private bool CurrentIsLineCommentOpenTag
            {
                get
                {
                    return Current == ';';
                }
            }

            private bool CurrentIsNewLine
            {
                get
                {
                    return Current == '\n';
                }
            }

            private bool CurrentIsWhiteSpace
            {
                get
                {
                    return Current == ' ' || Current == '\t';
                }
            }

            private bool CanMoveAgain
            {
                get;
                set;
            }

            private TextUnit LastAppended
            {
                get;
                set;
            }

            public IEnumerator<TextUnit> GetEnumerator()
            {
                LastAppended = new TextUnit(' ', -1, -1, -1, -1);
                while (MoveNext())
                {
                    if (TryGetEnumerable(out var collection))
                    {
                        foreach (var x in collection)
                        {
                            yield return x;
                        }
                    }
                    else
                    {
                        yield return LastAppended = Current;
                    }

                    if (!CanMoveAgain)
                    {
                        break;
                    }
                }
            }

            void IDisposable.Dispose()
            {
                BaseEnumerator.Dispose();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private bool TryGetEnumerable(out IEnumerable<TextUnit> collection)
            {
                collection = Special.TryGetValue(Current, out var func)
                    ? func()
                    : null;

                return collection != null;
            }

            private IEnumerable<TextUnit> AppendWhite()
            {
                if (LastAppended == ' ' || LastAppended == '\n')
                {
                    yield break;
                }

                yield return LastAppended = GetUnit(' ');
            }

            private bool MoveNext()
            {
                Previous = Current;
                return CanMoveAgain = BaseEnumerator.MoveNext();
            }

            private IEnumerable<TextUnit> TrimBlockComment()
            {
                if (!MoveNext())
                {
                    yield return Previous;
                }
                else if (Current != '*')
                {
                    yield return Previous;
                    yield return Current;
                }
                else if (!MoveNext())
                {
                    LogBlockCommentNotClosed();
                    yield break;
                }
                else
                {
                    foreach (var x in AppendWhite())
                    {
                        yield return x;
                    }

                    foreach (var x in TrimBlockCommentEnd())
                    {
                        yield return x;
                    }
                }
            }

            private IEnumerable<TextUnit> TrimBlockCommentEnd()
            {
                do
                {
                    if (Current == '*' && MoveNext() && Current == '/')
                    {
                        foreach (var x in AppendWhite())
                        {
                            yield return x;
                        }

                        yield break;
                    }
                    else if (!CanMoveAgain)
                    {
                        LogBlockCommentNotClosed();
                        yield break;
                    }
                    else if (Current == '\n')
                    {
                        yield return Current;
                    }
                }
                while (MoveNext());

                LogBlockCommentNotClosed();
                yield break;
            }

            private IEnumerable<TextUnit> TrimLineComment()
            {
                do
                {
                    if (!MoveNext())
                    {
                        yield break;
                    }
                }
                while (!CurrentIsNewLine);

                foreach (var x in AppendWhite())
                {
                    yield return x;
                }
            }

            private IEnumerable<TextUnit> AppendQuoteText()
            {
                yield return LastAppended = Current;
                if (MoveNext())
                {
                    while (!CurrentIsNewLine)
                    {
                        yield return Current;
                        if (Current == LastAppended)
                        {
                            yield break;
                        }
                    }
                }

                LogQuoteNotClosed();
                yield return GetUnit(LastAppended);
                yield break;
            }

            private IEnumerable<TextUnit> TrimWhiteSpace()
            {
                do
                {
                    if (!MoveNext())
                    {
                        yield break;
                    }
                }
                while (CurrentIsWhiteSpace);

                foreach (var x in AppendWhite())
                {
                    yield return x;
                }
            }

            private TextUnit GetUnit(char c)
            {
                return new TextUnit(
                    c,
                    Current.Index,
                    Current.Column,
                    Current.Line,
                    Current.File);
            }

            private TextUnit GetPrevious(char c)
            {
                return new TextUnit(
                    c,
                    Previous.Index,
                    Previous.Column,
                    Previous.Line,
                    Previous.File);
            }

            private TextUnit GetLastAppended(char c)
            {
                return new TextUnit(
                    c,
                    LastAppended.Index,
                    LastAppended.Column,
                    LastAppended.Line,
                    LastAppended.File);
            }

            private void LogBlockCommentNotClosed()
            {
                Logger.LogError(
                    0,
                    "C1035: End-of-file found, '*/' expected");
            }

            private void LogQuoteNotClosed()
            {
                Logger.LogError(
                    0,
                    "C1010: Quote string not closed.");
            }

            private string FormatMessage(string message)
            {
                return Owner.Path != null
                    ? $"{Owner.Path}({Current.Line + 1}): {message}"
                    : $"({Current.Line + 1}): {message}";
            }
        }
    }
}
