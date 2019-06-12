// <copyright file="Keywords.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Collections.Generic;

    internal class Keywords
    {
        private static readonly List<string> KeywordList = new List<string>()
        {
            "ADC",
            "AND",
            "ASL",
            "BCC",
            "BCS",
            "BEQ",
            "BIT",
            "BMI",
            "BNE",
            "BPL",
            "BRA",
            "BRK",
            "BRL",
            "BVC",
            "BVS",
            "CLC",
            "CLD",
            "CLI",
            "CLV",
            "CMP",
            "COP",
            "CPX",
            "CPY",
            "DEC",
            "DEX",
            "DEY",
            "EOR",
            "INC",
            "INX",
            "INY",
            "JML",
            "JMP",
            "JSL",
            "JSR",
            "LDA",
            "LDX",
            "LDY",
            "LSR",
            "MVN",
            "MVP",
            "NOP",
            "ORA",
            "PEA",
            "PEI",
            "PER",
            "PHA",
            "PHB",
            "PHD",
            "PHK",
            "PHP",
            "PHX",
            "PHY",
            "PLA",
            "PLB",
            "PLD",
            "PLP",
            "PLX",
            "PLY",
            "REP",
            "ROL",
            "ROR",
            "RTI",
            "RTL",
            "RTS",
            "SBC",
            "SEC",
            "SED",
            "SEI",
            "SEP",
            "STA",
            "STP",
            "STX",
            "STY",
            "STZ",
            "TAX",
            "TAY",
            "TCD",
            "TCS",
            "TDC",
            "TRB",
            "TSB",
            "TSC",
            "TSX",
            "TXA",
            "TXS",
            "TXY",
            "TYA",
            "TYX",
            "WAI",
            "WDM",
            "XBA",
            "XCE",
        };

        private static readonly HashSet<string> KeywordHashSet =
            new HashSet<string>(KeywordList, StringComparer.OrdinalIgnoreCase);

        public static bool IsOperator(string keyword)
        {
            return KeywordHashSet.Contains(keyword);
        }
    }
}
