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
            TransformTemplate template = new TransformTemplate()
            {
                Name = "housing",
                Inputs = new Dictionary<string, int>()
                    {
                        { "population", 5 },
                        { "metallicElements", 1 },
                        { "timber", 5 },
                        { "metallicAlloys", 3 },
                    },
                Outputs = new Dictionary<string, int>()
                    {
                        { "housing", 1 },
                        { "housingWaste", 1 },
                        { "population", 5 },
                    }
            };
            TransformTemplate deepCopy = template.DeepCopy();
            deepCopy.Should().BeEquivalentTo(template);
            deepCopy.Should().NotBeSameAs(template);
        }
    }
}
