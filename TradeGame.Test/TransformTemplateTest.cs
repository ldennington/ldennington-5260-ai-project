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
            TransformTemplate transformTemplate = new TransformTemplate()
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
                },
            };

            TransformTemplate deepCopy = transformTemplate.DeepCopy();
            deepCopy.Should().BeEquivalentTo(transformTemplate);
            deepCopy.Should().NotBeSameAs(transformTemplate);
        }
    }
}
