// <copyright file="Token.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    public class Token : IEquatable<Token>
    {
        public static readonly Token Eof = new Token(
            String.Empty,
            TokenType.Eof);

        public static readonly Token NewLine = new Token(
            "\n",
            TokenType.NewLine);

        public static readonly Token WhiteSpace = new Token(
            " ",
            TokenType.WhiteSpace);

        public static readonly Token LeftBrace = new Token(
            "{",
            TokenType.LeftBrace);

        public static readonly Token RightBrace = new Token(
            "}",
            TokenType.RightBrace);

        public static readonly Token LeftBracket = new Token(
            "[",
            TokenType.LeftBracket);

        public static readonly Token FileSeparator = new Token(
            "\\",
            TokenType.ForwardSlash);

        public static readonly Token RightBracket = new Token(
            "]",
            TokenType.RightBracket);

        public static readonly Token DirectAddress = new Token(
            "#",
            TokenType.Constant);

        public static readonly Token LeftParenthesis = new Token(
            "(",
            TokenType.LeftParenthesis);

        public static readonly Token RightParenthesis = new Token(
            ")",
            TokenType.RightParenthesis);

        public static readonly Token BitShiftLeft = new Token(
            "<<",
            TokenType.BitShiftLeft);

        public static readonly Token BitShiftRight = new Token(
            ">>",
            TokenType.BitShiftRight);

        public static readonly Token Modulo = new Token(
            "%",
            TokenType.Modulo);

        public static readonly Token BlockSeparator = new Token(
            ":",
            TokenType.BlockSeparator);

        public static readonly Token NameSpaceSeparator = new Token(
            "::",
            TokenType.NameSpaceSeparator);

        public static readonly Token OpSizeSeparator = new Token(
            ".",
            TokenType.OpSizeSeparator);

        public static readonly Token Multiplication = new Token(
            "*",
            TokenType.Multiplication);

        public static readonly Token Addition = new Token(
            "+",
            TokenType.Addition);

        public static readonly Token Subtration = new Token(
            "-",
            TokenType.Subtraction);

        public static readonly Token Division = new Token(
            "/",
            TokenType.Division);

        public static readonly Token BitwiseXor = new Token(
            "^",
            TokenType.BitwiseXor);

        public static readonly Token BitwiseAnd = new Token(
            "&",
            TokenType.BitwiseAnd);

        public static readonly Token BitwiseOr = new Token(
            "|",
            TokenType.BitwiseOr);

        public static readonly Token Negation = new Token(
            "=",
            TokenType.Negation);

        public static readonly Token Assignment = new Token(
            "=",
            TokenType.Assignment);

        public static readonly Token EquAssignment = new Token(
            "equ",
            TokenType.Assignment);

        public static readonly Token CommaSeparator = new Token(
            ",",
            TokenType.CommaSeparator);

        private static readonly HashSet<TokenType>
            NumberTypeHashSet = new HashSet<TokenType>()
            {
                TokenType.BinaryNumber,
                TokenType.DecimalNumber,
                TokenType.HexNumber,
            };

        private static readonly HashSet<TokenType>
            UnaryOperatorHashSet = new HashSet<TokenType>()
            {
                TokenType.Addition,
                TokenType.Subtraction,
                TokenType.Negation,
            };

        private static readonly HashSet<TokenType>
            BinaryOperatorHashSet = new HashSet<TokenType>()
            {
                TokenType.Addition,
                TokenType.Subtraction,
                TokenType.Multiplication,
                TokenType.Division,
                TokenType.Modulo,
                TokenType.BitShiftLeft,
                TokenType.BitShiftRight,
                TokenType.BitwiseAnd,
                TokenType.BitwiseOr,
                TokenType.BitwiseXor,
            };

        public Token(string text, TokenType tokenType)
        {
            if (!Enum.IsDefined(typeof(TokenType), tokenType))
            {
                throw new InvalidEnumArgumentException(
                    nameof(tokenType),
                    (int)tokenType,
                    typeof(TokenType));
            }

            Text = text
                ?? throw new ArgumentNullException(nameof(text));

            TokenType = tokenType;
        }

        public string Text
        {
            get;
        }

        public TokenType TokenType
        {
            get;
        }

        public bool IsNumberType
        {
            get
            {
                return NumberTypeHashSet.Contains(TokenType);
            }
        }

        public bool IsUnaryOperator
        {
            get
            {
                return UnaryOperatorHashSet.Contains(TokenType);
            }
        }

        public bool IsBinaryOperator
        {
            get
            {
                return BinaryOperatorHashSet.Contains(TokenType);
            }
        }

        public bool IsWhiteSpace
        {
            get
            {
                return
                    TokenType == TokenType.WhiteSpace ||
                    TokenType == TokenType.LineComment ||
                    TokenType == TokenType.BlockComment;
            }
        }

        public static string CollectionToString(
            IEnumerable<Token> collection)
        {
            var sb = new StringBuilder();
            foreach (var token in collection)
            {
                sb.Append(token);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return Text;
        }

        public bool Equals(Token other)
        {
            if (other is null)
            {
                return false;
            }

            if (TokenType != other.TokenType)
            {
                return false;
            }

            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (obj is Token other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.GetHashCode(Text) ^ (int)TokenType;
        }
    }
}
