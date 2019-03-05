using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Application.File
{
    public class DefaultImageSharpProcessActionAdapter : IImageSharpProcessActionAdapter
    {
        public Action<IImageProcessingContext<Rgba32>> Build(IList<string> expr)
        {
            // expr: h w resize h w y x crop m rotate
            // stack: [h, w]
            // stack: [h, w, y, x]
            // stack: [m]

            return !expr.Any() ? null : Build2(expr)?.Compile();
        }

        protected Expression<Action<IImageProcessingContext<Rgba32>>> Build2(IList<string> expr)
        {
            var builder = new ExpressionBuilder();
            Expression<Action<IImageProcessingContext<Rgba32>>> exp = null;
            var param = Expression.Parameter(typeof(IImageProcessingContext<Rgba32>), "x");
            var stack = new Stack<string>();
            foreach (var token in expr)
            {
                Expression call = null;
                switch (token)
                {
                    case "rotate":
                        var mode = (RotateMode) int.Parse(stack.Pop());
                        call = Expression.Call(typeof(RotateExtensions), "Rotate", new[] {typeof(Rgba32)}, param, Expression.Constant(mode));
                        break;

                    case "crop":
                        var x = int.Parse(stack.Pop());
                        var y = int.Parse(stack.Pop());
                        var w1 = int.Parse(stack.Pop());
                        var h1 = int.Parse(stack.Pop());
                        call = Expression.Call(typeof(CropExtensions), "Crop", new[] {typeof(Rgba32)}, param, Expression.Constant(new Rectangle(x, y, w1, h1)));
                        break;

                    case "resize":
                        var w2 = int.Parse(stack.Pop());
                        var h2 = int.Parse(stack.Pop());
                        call = Expression.Call(typeof(ResizeExtensions), "Resize", new[] {typeof(Rgba32)}, param,
                            Expression.Constant(new ResizeOptions {Position = AnchorPositionMode.Center, Size = new Size(w2, h2)}));
                        break;

                    default:
                        stack.Push(token);
                        break;
                }

                if (call != null)
                {
                    exp = exp != null
                        ? builder.Build(exp, Expression.Lambda<Action<IImageProcessingContext<Rgba32>>>(call, param))
                        : Expression.Lambda<Action<IImageProcessingContext<Rgba32>>>(call, param);
                }
            }

            return exp;
        }
    }
}