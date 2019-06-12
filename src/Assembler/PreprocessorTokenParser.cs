// <copyright file="PreprocessorTokenParser.cs" company="Public Domain">
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
    using static Grammar;

    public class PreprocessorTokenParser : IEnumerable<PreprocessorToken>
    {
        public PreprocessorTokenParser(
            IEnumerable<char> text,
            string path = null,
            int file = -1)
        {
            Text = new TrimWhitePreprocessor(
                text,
                path: path,
                file: file);
        }

        private TrimWhitePreprocessor Text
        {
            get;
        }

        public IEnumerator<PreprocessorToken> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var token in this)
            {
                sb.Append(token);
            }

            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class Builder :
            IEnumerable<PreprocessorToken>,
            IDisposable
        {
            private static readonly Dictionary<char, PreprocessorTokenType>
                PreprocessorTokenChars = new Dictionary<
                    char, PreprocessorTokenType>()
                {
                    { '\n', PreprocessorTokenType.NewLine },
                    { '{', PreprocessorTokenType.LeftBrace },
                    { '}', PreprocessorTokenType.RightBrace },
                    { '[', PreprocessorTokenType.LeftBracket },
                    { ']', PreprocessorTokenType.RightBracket },
                    { '(', PreprocessorTokenType.LeftParenthesis },
                    { ')', PreprocessorTokenType.RightParenthesis },
                    { '.', PreprocessorTokenType.Period },
                };

            public Builder(PreprocessorTokenParser owner)
            {
                Owner = owner
                    ?? throw new ArgumentNullException(nameof(owner));

                BaseEnumerator = Owner.Text.GetEnumerator();
                CurrentText = new StringBuilder();

                TokenDictionary = new Dictionary<
                    char, Func<PreprocessorTokenType>>()
                {
                    { ' ', ParseWhiteSpace },
                    { '_', ParseKeyword },
                    { '!', ParseDefine },
                    { '%', ParseBinaryOrModulo },
                };

                for (var c = 'a'; c <= 'z'; c++)
                {
                    TokenDictionary.Add(c, ParseKeyword);
                    TokenDictionary.Add((char)(c + 'A' - 'a'), ParseKeyword);
                }

                for (var c = '0'; c <= '9'; c++)
                {
                    TokenDictionary.Add(c, ParseDecimal);
                }

                foreach (var c in PreprocessorTokenChars.Keys)
                {
                    TokenDictionary.Add(c, () => ParseCharacter(c));
                }
            }

            private Dictionary<char, Func<PreprocessorTokenType>>
                TokenDictionary
            {
                get;
            }

            private PreprocessorTokenParser Owner
            {
                get;
            }

            private TextUnit Current
            {
                get
                {
                    return BaseEnumerator.Current;
                }
            }

            private TextUnit First
            {
                get;
                set;
            }

            private StringBuilder CurrentText
            {
                get;
            }

            private IEnumerator<TextUnit> BaseEnumerator
            {
                get;
            }

            private bool CanMoveAgain
            {
                get;
                set;
            }

            public IEnumerator<PreprocessorToken> GetEnumerator()
            {
                MoveNext();
                while (CanMoveAgain)
                {
                    First = Current;
                    CurrentText.Clear();
                    var tokenType = PreprocessorTokenType.InvalidSequence;
                    if (IsNonDigit(Current))
                    {
                        tokenType = ParseRule(
                            IsNonDigit,
                            PreprocessorTokenType.Keyword);
                    }
                    else if (Current == '+')
                    {
                        tokenType = ParseCharacter('+');
                    }
                    else if (IsDigit(Current))
                    {
                        tokenType = ParseRule(
                            IsDigit,
                            PreprocessorTokenType.DecimalNumber);
                    }

                    if (CanMoveAgain)
                    {
                        CurrentText.Remove(CurrentText.Length - 1, 1);
                    }

                    yield return new PreprocessorToken(
                        CurrentText.ToString(),
                        First,
                        tokenType);
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

            private PreprocessorTokenType ParseKeyword()
            {
                return ParseRule(IsNonDigit, PreprocessorTokenType.Keyword);
            }

            private PreprocessorTokenType ParseDefine()
            {
                return ParseRule(IsNonDigit, PreprocessorTokenType.Define);
            }

            private PreprocessorTokenType ParseBinaryOrModulo()
            {
                if (MoveNext() && IsBinaryDigit(Current))
                {
                    return ParseRule(
                        IsBinaryDigit,
                        PreprocessorTokenType.DecimalNumber);
                }

                return PreprocessorTokenType.Modulo;
            }

            private PreprocessorTokenType ParseDecimal()
            {
                return ParseRule(IsDigit, PreprocessorTokenType.DecimalNumber);
            }

            private PreprocessorTokenType ParseWhiteSpace()
            {
                return ParseRule(
                    IsWhiteSpace,
                    PreprocessorTokenType.WhiteSpace);
            }

            private PreprocessorTokenType ParseRule(
                Func<char, bool> rule,
                PreprocessorTokenType preprocessorTokenType)
            {
                while (MoveNext())
                {
                    if (!rule(Current))
                    {
                        break;
                    }
                }

                return preprocessorTokenType;
            }

            private PreprocessorTokenType ParseCharacter(char c)
            {
                MoveNext();
                return PreprocessorTokenChars[c];
            }

            private bool MoveNext()
            {
                if (CanMoveAgain = BaseEnumerator.MoveNext())
                {
                    CurrentText.Append(Current);
                }

                return CanMoveAgain;
            }
        }
    }
}
