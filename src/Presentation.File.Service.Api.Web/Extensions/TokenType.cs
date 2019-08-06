namespace Presentation.File.Service.Api.Web.Extensions
{
    public enum TokenType
    {
        Unknown = 0,
        OpenParentheses,
        CloseParentheses,
        Comma,
        Dot,
        Method,
        Literal,
        ExprEnd
    }
}