using System;
using System.Collections.Generic;
using System.IO;

namespace Presentation.File.Service.Api.Web.Extensions
{
    public class Scanner
    {
        public static Tuple<IList<Token>, IDictionary<Token, Symbol>> Scan(string expr, IList<string> methods)
        {
            var index = 0;
            var reader = new StringReader(expr);
            var tokens = new List<Token>();
            var symbols = new Dictionary<Token, Symbol>();
            Token token;
            do
            {
                token = new Token(ref index, reader, methods);
                tokens.Add(token);

                if (token.Type == TokenType.Method)
                {
                    symbols.Add(token, new Symbol(token.Value));
                }
                else if (token.Type == TokenType.Literal)
                {
                    symbols.Add(token, new Symbol(token.Value, typeof(int))); // 只有int
                }

                SkipWhiteSpace(ref index, reader);
            } while (token.Type != TokenType.ExprEnd);

            return new Tuple<IList<Token>, IDictionary<Token, Symbol>>(tokens, symbols);
        }

        private static void SkipWhiteSpace(ref int i, StringReader reader)
        {
            var next = reader.Peek();
            while ((char) next == ' ')
            {
                reader.Read();
                i++;
                next = reader.Peek();
            }
        }
    }
}