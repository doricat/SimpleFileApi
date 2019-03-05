using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Application.File.Tests
{
    public class DefaultImageSharpProcessActionAdapterTests
    {
        public class FakeDefaultImageSharpProcessActionAdapter : DefaultImageSharpProcessActionAdapter
        {
            public new Expression<Action<IImageProcessingContext<Rgba32>>> Build2(IList<string> expr)
            {
                return base.Build2(expr);
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var adapter = new FakeDefaultImageSharpProcessActionAdapter();
            var expStr = "100 100 resize 100 100 0 0 crop 90 rotate";
            var result = adapter.Build2(expStr.Split(' '));
            Console.WriteLine(result.ToString());
        }

        [Test]
        public void Test2()
        {
            var adapter = new FakeDefaultImageSharpProcessActionAdapter();
            var expStr = "270 rotate 100 100 0 0 crop";
            var result = adapter.Build2(expStr.Split(' '));
            Console.WriteLine(result.ToString());
        }
    }
}