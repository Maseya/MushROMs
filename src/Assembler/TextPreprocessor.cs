// <copyright file="TextPreprocessor.cs" company="Public Domain">
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

    public class TextPreprocessor : IEnumerable<TextUnit>
    {
        private static readonly Dictionary<char, char> TrigraphDictionary =
            new Dictionary<char, char>()
            {
                { '=', '#' },
                { ')', ']' },
                { '!', '|' },
                { '(', '[' },
                { '\'', '^' },
                { '>', '}' },
                { '/', '\\' },
                { '<', '{' },
                { '-', '~' },
            };

        public TextPreprocessor(
            IEnumerable<char> text,
            bool resolveTrigraphs = false,
            bool forceAscii = false,
            bool addTrailingNewLine = false,
            bool trimTrailingNewLine = false,
            int file = -1)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            ResolveTrigraphs = resolveTrigraphs;
            ForceAscii = forceAscii;
            AddTrailingNewLine = addTrailingNewLine;
            TrimTrailingNewLine = trimTrailingNewLine;
            File = file;
        }

        public int File
        {
            get;
        }

        public bool AddTrailingNewLine
        {
            get;
            set;
        }

        public bool ForceAscii
        {
            get;
            set;
        }

        public bool ResolveTrigraphs
        {
            get;
            set;
        }

        public bool TrimTrailingNewLine
        {
            get;
            set;
        }

        private IEnumerable<char> Text
        {
            get;
        }

        public static bool IsTrigraphChar(char c)
        {
            return TrigraphDictionary.ContainsKey(c);
        }

        public static bool TryGetTrigraph(char c, out char trigraph)
        {
            return TrigraphDictionary.TryGetValue(c, out trigraph);
        }

        public IEnumerator<TextUnit> GetEnumerator()
        {
            using (var builder = new Builder(this))
            using (var en = builder.GetEnumerator())
            {
                // Only process text that is not empty.
                if (en.MoveNext())
                {
                    var queue = new Queue<TextUnit>();

                _loop:
                    var last = en.Current;
                    if (!en.MoveNext())
                    {
                        foreach (var c in FinalizeNewLines(last, queue))
                        {
                            yield return c;
                        }

                        yield break;
                    }

                    if (last == '\n')
                    {
                        queue.Enqueue(last);
                    }
                    else
                    {
                        while (queue.Count > 0)
                        {
                            yield return queue.Dequeue();
                        }

                        yield return last;
                    }

                    goto _loop;
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var unit in this)
            {
                sb.Append(unit.Value);
            }

            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<TextUnit> FinalizeNewLines(
            TextUnit last,
            Queue<TextUnit> queue)
        {
            if (TrimTrailingNewLine)
            {
                if (last != '\n')
                {
                    yield return last;
                }

                if (AddTrailingNewLine)
                {
                    yield return TrailingNewLine();
                }
            }
            else
            {
                while (queue.Count > 0)
                {
                    yield return queue.Dequeue();
                }

                yield return last;
                if (last != '\n' && AddTrailingNewLine)
                {
                    yield return TrailingNewLine();
                }
            }

            TextUnit TrailingNewLine()
            {
                return new TextUnit(
                    '\n',
                    last.Index + 1,
                    last.Column + 1,
                    last.Line,
                    last.File);
            }
        }

        private class Builder : IEnumerable<TextUnit>, IDisposable
        {
            public Builder(TextPreprocessor owner)
            {
                Owner = owner
                    ?? throw new ArgumentNullException(nameof(owner));

                BaseEnumerator = Owner.Text.GetEnumerator();
                Special =
                    new Dictionary<char, Func<IEnumerable<TextUnit>>>()
                {
                    { '\r', ConvertToNewLine },
                    { '?', ConvertTrigraph },
                };
            }

            private IEnumerator<char> BaseEnumerator
            {
                get;
            }

            private char Current
            {
                get
                {
                    return BaseEnumerator.Current;
                }
            }

            private Dictionary<char, Func<IEnumerable<TextUnit>>> Special
            {
                get;
            }

            private bool CanMoveAgain
            {
                get;
                set;
            }

            private int Index
            {
                get;
                set;
            }

            private int Line
            {
                get;
                set;
            }

            private int Column
            {
                get;
                set;
            }

            private int File
            {
                get
                {
                    return Owner.File;
                }
            }

            private bool ForceAscii
            {
                get
                {
                    return Owner.ForceAscii;
                }
            }

            private bool IsNewLine
            {
                get
                {
                    return Current == '\n';
                }
            }

            private bool IsReturnChar
            {
                get
                {
                    return Current == '\r';
                }
            }

            private TextPreprocessor Owner
            {
                get;
            }

            private bool ResolveTrigraphs
            {
                get
                {
                    return Owner.ResolveTrigraphs;
                }
            }

            public IEnumerator<TextUnit> GetEnumerator()
            {
                MoveNext();
                while (CanMoveAgain)
                {
                    if (TryGetEnumerable(out var collection))
                    {
                        foreach (var unit in collection)
                        {
                            yield return unit;
                        }
                    }
                    else
                    {
                        yield return GetUnitAndAdvance(Current);
                        if (Current == '\n')
                        {
                            Line++;
                            Column = 0;
                        }

                        MoveNext();
                    }
                }
            }

            void IDisposable.Dispose()
            {
                BaseEnumerator?.Dispose();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private bool TryGetEnumerable(out IEnumerable<TextUnit> collection)
            {
                collection = Special.TryGetValue(Current, out var func)
                    ? func()
                    : Current > 0x7F && ForceAscii
                    ? EnumerateCharEscape()
                    : null;

                return collection != null;
            }

            private IEnumerable<TextUnit> ConvertToNewLine()
            {
                yield return GetUnit('\n');
                Index++;
                Line++;
                Column = 0;
                if (MoveNext() && IsNewLine)
                {
                    Index++;
                    MoveNext();
                }
            }

            private IEnumerable<TextUnit> ConvertTrigraph()
            {
                if (IsTrigraphCandidate())
                {
                _loop:
                    if (!MoveNext())
                    {
                        yield return GetUnitAndAdvance('?');
                        yield return GetUnitAndAdvance('?');
                    }
                    else if (Current == '?')
                    {
                        yield return GetUnitAndAdvance('?');
                        goto _loop;
                    }
                    else if (TryGetTrigraph(Current, out var trigraph))
                    {
                        yield return GetUnit(trigraph);
                        Index += 3;
                        Column += 3;
                        MoveNext();
                    }
                    else
                    {
                        yield return GetUnitAndAdvance('?');
                        yield return GetUnitAndAdvance('?');
                    }
                }
                else
                {
                    yield return GetUnitAndAdvance('?');
                }
            }

            private bool IsTrigraphCandidate()
            {
                return ResolveTrigraphs && MoveNext() && Current == '?';
            }

            private IEnumerable<TextUnit> EnumerateCharEscape()
            {
                yield return GetUnit('\\');
                yield return GetUnit('u');
                for (var i = 0x10; (i -= 4) >= 0;)
                {
                    yield return GetUnit(GetChar((Current >> i) & 0x0F));
                }

                Index++;
                MoveNext();

                char GetChar(int value)
                {
                    return (char)(value < 10
                        ? value + '0'
                        : value - 10 + 'A');
                }
            }

            private bool MoveNext()
            {
                return CanMoveAgain = BaseEnumerator.MoveNext();
            }

            private TextUnit GetUnit(char c)
            {
                return new TextUnit(
                    c,
                    Index,
                    Column,
                    Line,
                    File);
            }

            private TextUnit GetUnitAndAdvance(char c)
            {
                return new TextUnit(
                    c,
                    Index++,
                    Column++,
                    Line,
                    File);
            }
        }
    }
}
