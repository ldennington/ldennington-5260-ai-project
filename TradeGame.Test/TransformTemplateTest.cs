using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TradeGame.Test
{
    [TestClass]
    public class TransformTemplateTest
    {
        [TestMethod]
        public void DeepCopy()
        {
            TransformTemplate deepCopy = TestData.TRANSFORM_TEMPLATE.DeepCopy();
            deepCopy.Should().BeEquivalentTo(TestData.TRANSFORM_TEMPLATE);
            deepCopy.Should().NotBeSameAs(TestData.TRANSFORM_TEMPLATE);
        }
    }
}
