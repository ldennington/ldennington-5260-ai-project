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
                Inputs = new Dictionary<Resource, int>()
                    {
                        { new Resource { Name = "population" }, 5 },
                        { new Resource { Name = "metallicElements" }, 1 },
                        { new Resource { Name = "timber" }, 5 },
                        { new Resource { Name = "metallicAlloys" }, 3 },
                    },
                Outputs = new Dictionary<Resource, int>()
                    {
                        { new Resource { Name = "housing" }, 1 },
                        { new Resource { Name = "housingWaste" }, 1 },
                        { new Resource { Name = "population" }, 5 },
                    },
            };
            TransformTemplate deepCopy = template.DeepCopy();
            deepCopy.Should().BeEquivalentTo(template);
            deepCopy.Should().NotBeSameAs(template);
        }
    }
}
