using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TradeGame.Test
{
    [TestClass]
    public class TransformTemplateTest
    {
        private TransformTemplate transformTemplate;

        [TestInitialize]
        public void Initialize()
        {
            transformTemplate = new TransformTemplate()
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
        }

        [TestCleanup]
        public void Cleanup()
        {
            // reset for setup of next test
            transformTemplate = new TransformTemplate();
        }

        [TestMethod]
        public void DeepCopy()
        {
            TransformTemplate deepCopy = transformTemplate.DeepCopy();
            deepCopy.Should().BeEquivalentTo(transformTemplate);
            deepCopy.Should().NotBeSameAs(transformTemplate);
            deepCopy.Inputs.Should().NotBeSameAs(transformTemplate.Inputs);
            deepCopy.Outputs.Should().NotBeSameAs(transformTemplate.Outputs);
        }

        [TestMethod]
        public void SetScale()
        {
            Country country = new Country()
            {
                Name = "Atlantis",
                State = new Dictionary<string, int>()
                {
                    { "population", 50 },
                    { "metallicElements", 300 },
                    { "timber", 1200 },
                    { "metallicAlloys", 15 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
            };

            transformTemplate.SetScale(country);
            transformTemplate.Scale.Should().Be(5);

            transformTemplate.Inputs["population"].Should().Be(25);
            transformTemplate.Inputs["metallicElements"].Should().Be(5);
            transformTemplate.Inputs["timber"].Should().Be(25);
            transformTemplate.Inputs["metallicAlloys"].Should().Be(15);

            transformTemplate.Outputs["housing"].Should().Be(5);
            transformTemplate.Outputs["housingWaste"].Should().Be(5);
            transformTemplate.Outputs["population"].Should().Be(25);
        }
    }
}
