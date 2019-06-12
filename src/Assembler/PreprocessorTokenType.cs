// <copyright file="PreprocessorTokenType.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    public enum PreprocessorTokenType
    {
        NewLine,
        WhiteSpace,

        Pragma,
        WhiteSpaceDelete,
        Define,
        Assignment,
        ConditionalAssignment,
        MathAssignment,
        ResolvedAssignment,
        MacroArg,
        MacroCall,
        MacroSublabelDefine,
        MacroSublabelCall,
        Comma,
        Period,

        BinaryNumber,
        DecimalNumber,
        HexadecimalNumber,

        LeftBrace,
        RightBrace,
        LeftBracket,
        RightBracket,
        LeftParenthesis,
        RightParenthesis,

        Addition,
        Subtraction,
        Multiplcation,
        Division,
        Modulo,

        BinaryNegation,
        BitShiftLeft,
        BitShiftRight,
        BinaryAnd,
        BinaryOr,
        BinaryXor,

        LogicalNot,
        LogicalAnd,
        LogicalOr,
        LogicalXor = BinaryXor,

        EqualTo = LogicalOr + 1,
        LessThan,
        GreaterThan,
        LessThanOrEqualTo,
        GreaterThanOrEqualTo,

        Keyword,
        StringLiteral,

        InvalidSequence,
    }
}
