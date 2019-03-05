using System;
using System.Linq.Expressions;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Application.File
{
    public class ExpressionBuilder : ExpressionVisitor
    {
        private Expression _currentExp;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _currentExp;
        }

        public Expression<Action<IImageProcessingContext<Rgba32>>> Build(Expression<Action<IImageProcessingContext<Rgba32>>> exp1,
            Expression<Action<IImageProcessingContext<Rgba32>>> exp2)
        {
            _currentExp = exp1.Body;
            var newBody = Visit(exp2.Body);
            return Expression.Lambda<Action<IImageProcessingContext<Rgba32>>>(newBody ?? throw new InvalidOperationException(), exp1.Parameters[0]);
        }
    }
}