using System.Collections.Generic;
using System.Linq;
using Presentation.File.Service.Api.Web.Extensions;

namespace Presentation.File.Service.Api.Web.ViewModels
{
    public class Analyzer
    {
        public static IList<string> Analyze(IList<Token> tokens, IDictionary<Token, Symbol> symbols)
        {
            var errors = new List<string>();
            errors.AddRange(tokens.Where(x => x.Type == TokenType.Unknown).Select(x => $"包含意外字符 {x.Value} ({x.Index})"));
            if (!errors.Any() && TryCheckSyntax(tokens, symbols, ref errors))
            {
                return errors;
            }

            return errors;
        }

        private static bool TryCheckSyntax(IList<Token> tokens, IDictionary<Token, Symbol> symbols, ref List<string> errors)
        {
            for (var i = 0; i < tokens.Count - 1; i++)
            {
                var token = tokens[i];
                var next = tokens[i + 1];
                var l = errors.Count;

                switch (token.Type)
                {
                    case TokenType.OpenParentheses:
                    case TokenType.Comma:
                        if (next.Type != TokenType.Literal)
                        {
                            errors.Add($"表达式项 {next.Value} ({next.Index}) 无效，应使用字面量");
                        }
                        break;
                    case TokenType.CloseParentheses:
                        if (next.Type != TokenType.Dot && next.Type != TokenType.ExprEnd)
                        {
                            errors.Add($"表达式项 {next.Value} ({next.Index}) 无效，应使用'.'");
                        }
                        break;
                    case TokenType.Dot:
                        if (next.Type != TokenType.Method)
                        {
                            errors.Add($"表达式项 {next.Value} ({next.Index}) 无效，应使用方法名称");
                        }
                        break;
                    case TokenType.Method: // TODO 检查方法参数个数
                        if (next.Type != TokenType.OpenParentheses)
                        {
                            errors.Add($"表达式项 {next.Value} ({next.Index}) 无效，应使用(");
                        }
                        break;
                    case TokenType.Literal:
                        if (next.Type != TokenType.CloseParentheses && next.Type != TokenType.Comma)
                        {
                            errors.Add($"表达式项 {next.Value} ({next.Index}) 无效，应使用,或)");
                        }
                        else
                        {
                            if (!int.TryParse(token.Value, out _))
                            {
                                errors.Add($"表达式项 {next.Value} ({next.Index}) 无效，不能转换到int类型");
                            }
                        }
                        break;
                }

                if (errors.Count != l)
                {
                    i++;
                }
            }

            return !errors.Any();
        }
    }
}