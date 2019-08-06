using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Presentation.File.Service.Api.Web.Extensions
{
    public class Token
    {
        private static readonly IDictionary<char, TokenType> DelimitersTypes = new Dictionary<char, TokenType>
        {
            {'(', TokenType.OpenParentheses},
            {')', TokenType.CloseParentheses},
            {',', TokenType.Comma},
            {'.', TokenType.Dot}
        };

        public Token(ref int index, StringReader reader, IList<string> methods)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (methods == null || !methods.Any()) throw new ArgumentNullException(nameof(methods));

            var value = reader.Read();
            index++;
            if (value == -1)
            {
                Type = TokenType.ExprEnd;
                return;
            }

            var c = (char) value;
            if (DelimitersTypes.ContainsKey(c))
            {
                Type = DelimitersTypes[c];
                Value = c.ToString();
                Index = index;
            }
            else
            {
                var list = new List<char>(20) {c};
                var nextValue = reader.Peek();
                while (nextValue != -1 && !DelimitersTypes.ContainsKey((char) nextValue) && (char) nextValue != ' ')
                {
                    list.Add((char) reader.Read());
                    index++;
                    nextValue = reader.Peek();
                }

                Index = index;
                Value = new string(list.ToArray());
                if (Regex.IsMatch(Value, @"^\d+$"))
                {
                    Type = TokenType.Literal;
                }
                else
                {
                    Type = methods.Contains(Value) ? TokenType.Method : TokenType.Unknown;
                }
            }
        }

        public TokenType Type { get; }

        public string Value { get; }

        public int? Index { get; }
    }
}