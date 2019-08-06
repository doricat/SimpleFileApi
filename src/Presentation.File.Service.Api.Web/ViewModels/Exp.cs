using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Presentation.File.Service.Api.Web.Extensions;

namespace Presentation.File.Service.Api.Web.ViewModels
{
    [ModelBinder(typeof(ExpModelBinder))]
    public class Exp : IValidatableObject
    {
        private static readonly IList<string> Methods = new List<string> {"resize", "crop", "rotate"};

        public Exp(string action)
        {
            Action = action;

            if (!string.IsNullOrWhiteSpace(action))
            {
                var (tokens, symbols) = Scanner.Scan(action, Methods);
                Tokens = tokens;
                Symbols = symbols;
                var errors = Analyzer.Analyze(tokens, symbols);
                if (errors.Any())
                {
                    Errors.AddRange(errors);
                }
            }
        }

        /// <summary>
        /// eg:resize(w,h).crop(x,y,w,h).rotate(90)
        /// </summary>
        public string Action { get; }

        public IList<Token> Tokens { get; }

        public IDictionary<Token, Symbol> Symbols { get; }

        public List<string> Errors { get; } = new List<string>();

        public IList<string> ToExp()
        {
            var result = new List<string>();
            var temp = new List<string>();
            string method = null;
            foreach (var token in Tokens)
            {
                if (token.Type == TokenType.Method || token.Type == TokenType.ExprEnd)
                {
                    if (temp.Any() && method != null)
                    {
                        temp.Reverse();
                        result.AddRange(temp);
                        result.Add(method);
                        temp.Clear();
                    }

                    method = token.Value;
                    continue;
                }

                if (token.Type == TokenType.Literal)
                {
                    temp.Add(token.Value);
                }
            }

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Errors.Any())
            {
                result.AddRange(Errors.Select(x => new ValidationResult(x)));
            }

            return result;
        }
    }
}