// <copyright file="Grammar.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Collections.Generic;

    internal static class Grammar
    {
        private static readonly IReadOnlyDictionary<char, char>
            TrigraphDictionary = new Dictionary<char, char>()
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

        public static bool IsEOF(char c)
        {
            return c == '\0';
        }

        public static bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        public static bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        public static bool IsAlpha(char c)
        {
            return IsLower(c) || IsUpper(c);
        }

        public static bool IsNonDigit(char c)
        {
            return IsAlpha(c) || c == '_';
        }

        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsBinaryDigit(char c)
        {
            return c == '0' || c == '1';
        }

        public static bool IsHexDigit(char c)
        {
            return IsDigit(c)
                || IsUpperHexAlpha(c)
                || IsLowerHexAlpha(c);
        }

        public static bool IsUpperHexAlpha(char c)
        {
            return c >= 'A' && c <= 'F';
        }

        public static bool IsLowerHexAlpha(char c)
        {
            return c >= 'a' && c <= 'f';
        }

        public static bool IsAlphaNum(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        public static bool IsKeywordChar(char c)
        {
            return IsNonDigit(c) || IsDigit(c);
        }

        public static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t';
        }

        public static bool IsSingleQuote(char c)
        {
            return c == '\'';
        }

        public static bool IsDoubleQuote(char c)
        {
            return c == '\"';
        }

        public static bool IsQuote(char c)
        {
            return IsSingleQuote(c) || IsDoubleQuote(c);
        }

        public static bool IsLineSeparator(char c)
        {
            return c == '\r' || c == '\n';
        }

        public static int ParseHexDigit(char c)
        {
            if (IsDigit(c))
            {
                return c - '0';
            }

            if (IsUpperHexAlpha(c))
            {
                return c - 'A';
            }

            if (IsLowerHexAlpha(c))
            {
                return c - 'a';
            }

            throw new ArgumentException(nameof(c));
        }

        public static bool IsTrigraphChar(char c)
        {
            return TrigraphDictionary.ContainsKey(c);
        }

        public static bool TryGetTrigraph(char c, out char trigraph)
        {
            return TrigraphDictionary.TryGetValue(c, out trigraph);
        }
    }
}
