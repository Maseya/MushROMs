// <copyright file="TokenType.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    public enum TokenType
    {
        /// <summary>
        /// A token of the EOF char '\0'.
        /// </summary>
        Eof,

        /// <summary>
        /// Any newline. A regex match to "(\n|\r(\n)?)".
        /// </summary>
        NewLine,

        /// <summary>
        /// Any whitespace. A regex match to "\s+".
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// Any line comment starting with ';'. A regex match to ";.*$";
        /// </summary>
        LineComment,

        /// <summary>
        /// Any block comment starting with "/*" and ending with "*/". If a
        /// newline is found before "*/", a <see cref="NewLine"/> token is
        /// inserted and a new <see cref="BlockComment"/> is created after.
        /// </summary>
        BlockComment,

        /// <summary>
        /// Any sequence starting with [_a-zA-Z] and a repeating sequence of
        /// [_a-zA-Z0-9] that does not end with ':'. Any sequence with regex
        /// match "[_a-zA-Z]\w*^:".
        /// </summary>
        Keyword,

        /// <summary>
        /// Any <see cref="Keyword"/> sequence that ends with ':' and does not
        /// start with '?'. A regex match to "[^\?][_a-zA-Z]\w*:".
        /// </summary>
        Label,

        /// <summary>
        /// The single character '.'.
        /// </summary>
        OpSizeSeparator,

        /// <summary>
        /// Any <see cref="Keyword"/> whose text is "macro".
        /// </summary>
        MacroDefine,

        /// <summary>
        /// Any <see cref="Keyword"/> whose text is "endmacro".
        /// </summary>
        MacroClose,

        /// <summary>
        /// A char '=' or a string "equ".
        /// </summary>
        Assignment,

        /// <summary>
        /// The sequence "==".
        /// </summary>
        LogicalEquality,

        /// <summary>
        /// The single char '@'.
        /// </summary>
        Pragma,

        /// <summary>
        /// Any sequence of "\d+".
        /// </summary>
        DecimalNumber,

        /// <summary>
        /// The char '{'.
        /// </summary>
        LeftBrace,

        /// <summary>
        /// The char '}'.
        /// </summary>
        RightBrace,

        /// <summary>
        /// The char '['.
        /// </summary>
        LeftBracket,

        /// <summary>
        /// The char '/'.
        /// </summary>
        ForwardSlash,

        /// <summary>
        /// The char ']'.
        /// </summary>
        RightBracket,

        /// <summary>
        /// The char '#'. Only appears when specifying an ASM direct value
        /// (e.g. LDA #00).
        /// </summary>
        Constant,

        /// <summary>
        /// The char '('.
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// The char ')'.
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// Any sequence that starts with '&lt;', followed by a <see
        /// cref="Keyword"/>, and ending with '&gt;'. A regex match to
        /// "&lt;[_a-zA-Z]\w*&gt;".
        /// </summary>
        MacroArg,

        /// <summary>
        /// The single char '?'.
        /// </summary>
        Conditional,

        /// <summary>
        /// The sequence "&gt;&gt;".
        /// </summary>
        BitShiftLeft,

        /// <summary>
        /// The sequence "&lt;&lt;".
        /// </summary>
        BitShiftRight,

        /// <summary>
        /// The single char '%'.
        /// </summary>
        Modulo,

        /// <summary>
        /// Any sequence starting with '%' followed by binary digits. A regex
        /// match to "%[01]+".
        /// </summary>
        BinaryNumber,

        MacroCall,

        /// <summary>
        /// Any sequence starting with '$' followed by hexadecimal digits. A
        /// regex match to "\$[0-9a-fA-F]".
        /// </summary>
        HexNumber,

        /// <summary>
        /// The single char ':' with whitespace on either side. A regex match
        /// to "(^|\s+):(\s+|$)".
        /// </summary>
        BlockSeparator,

        /// <summary>
        /// The sequence "::".
        /// </summary>
        NameSpaceSeparator,

        /// <summary>
        /// The sequence "!=".
        /// </summary>
        NotEqualTo,

        /// <summary>
        /// The single char '&gt;'.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// The sequence "&gt;=".
        /// </summary>
        GreaterThanEqualTo,

        /// <summary>
        /// The single char '~'.
        /// </summary>
        BitwiseNot,

        /// <summary>
        /// The sequence "?=".
        /// </summary>
        ConditionalAssignment,

        /// <summary>
        /// The sequence "#=".
        /// </summary>
        MathAssignment,

        /// <summary>
        /// The sequence ":=".
        /// </summary>
        ResolvedAssignment,

        /// <summary>
        /// The sequence "+=".
        /// </summary>
        AppendAssignment,

        Sublabel,

        /// <summary>
        /// The single char '.'.
        /// </summary>
        KeywordSeparator,

        OpSizeSpecifier,
        SublabelOrOpSizeSpecifier,

        /// <summary>
        /// The character '?' followed by a <see cref="Label"/>. A regex match
        /// to "?[_a-zA-Z]\w*:".
        /// </summary>
        MacroLabelDefine,

        /// <summary>
        /// e.g. "?loop".
        /// </summary>
        MacroSublabelCall,

        /// <summary>
        /// The single char '*'.
        /// </summary>
        Multiplication,

        /// <summary>
        /// The single char '&lt;'.
        /// </summary>
        LessThan,

        /// <summary>
        /// The sequence "&lt;=".
        /// </summary>
        LessThanOrEqualTo,

        /// <summary>
        /// The single char '+'.
        /// </summary>
        Addition,

        ForwardLabel,
        AdditionOrForwardLabel,

        /// <summary>
        /// The single char '-'.
        /// </summary>
        Subtraction,

        BackwardLabel,
        SubtractionOrBackwardLabel,

        /// <summary>
        /// The single char '/'.
        /// </summary>
        Division,

        /// <summary>
        /// The single char '^'.
        /// </summary>
        BitwiseXor,

        /// <summary> The single char '&'. </summary>
        BitwiseAnd,

        /// <summary>
        /// The single char '|'.
        /// </summary>
        BitwiseOr,

        /// <summary>
        /// The single char '~'.
        /// </summary>
        Negation,

        /// <summary>
        /// The single char '^'.
        /// </summary>
        LogicalXor,

        /// <summary> The sequence "&&". </summary>
        LogicalAnd,

        /// <summary>
        /// The sequence "||".
        /// </summary>
        LogicalOr,

        /// <summary>
        /// The single char '!'.
        /// </summary>
        LogicalNot,

        Define,

        /// <summary>
        /// The single char ','.
        /// </summary>
        CommaSeparator,

        /// <summary>
        /// A sequence of characters between quotes.
        /// </summary>
        String,

        /// <summary>
        /// Any sequence of characters that does not form a one of the other
        /// valid <see cref="TokenType"/> values.
        /// </summary>
        InvalidCharSequence,
    }
}
