// <copyright file="PreprocessorToken.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;

    public class PreprocessorToken
    {
        public PreprocessorToken(
            string text,
            TextUnit first,
            PreprocessorTokenType preprocessorTokenType)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            First = first;
            PreprocessorTokenType = preprocessorTokenType;
        }

        public PreprocessorTokenType PreprocessorTokenType
        {
            get;
        }

        public TextUnit First
        {
            get;
        }

        private string Text
        {
            get;
        }

        public static implicit operator string(PreprocessorToken token)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token.Text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
