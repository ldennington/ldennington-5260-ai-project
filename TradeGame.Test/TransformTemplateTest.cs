using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TradeGame.Test
{
    [TestClass]
    public class TransformTemplateTest
    {
        private TransformTemplate transformTemplate;
        private Country country;
        private Node initialNode;

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

            country = new Country()
            {
                Name = "Atlantis",
                State = new Dictionary<string, int>()
                {
                    { "population", 50 },
                    { "metallicElements", 300 },
                    { "timber", 1200 },
                    { "metallicAlloys", 3 },
                    { "electronics", 0 },
                    { "housing", 0 }
                }
            };

            initialNode = new Node()
            {
                State = new List<Country>()
                {
                    new Country() { Name = "Atlantis", State = new Dictionary<string, int>()
                        {
                            { "population",  100 },
                            { "metallicElements",  700 },
                            { "timber",  2000 },
                            { "metallicAlloys", 9 },
                            { "housing", 0 },
                            { "housingWaste", 0 },
                        },
                        IsSelf = true
                    },
                    new Country() { Name = "Brobdingnag", State = new Dictionary<string, int>()
                        {
                            { "population", 50 },
                            { "metallicElements", 300 },
                            { "timber", 1200 },
                            { "metallicAlloys", 3 },
                            { "electronics", 0 },
                            { "housing", 0 }
                        }
                    },
                }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            // reset for setup of next test
            transformTemplate = new TransformTemplate();
            country = new Country();
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

        [TestMethod]
        public void Execute()
        {
            Country expected = new Country()
            {
                Name = "Atlantis",
                State = new Dictionary<string, int>()
                {
                    { "population", 50 },
                    { "metallicElements", 299 },
                    { "timber", 1195 },
                    { "metallicAlloys", 0 },
                    { "electronics", 0 },
                    { "housing", 1 },
                    { "housingWaste", 1 },
                }
            };
            transformTemplate.Scale = 1;
            transformTemplate.Execute(country);
            country.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void EnsureDeepCopyDoesNotModifyInitialState()
        {
            Node copy = initialNode.DeepCopy();

            Country initialSelf = initialNode.State.Where(c => c.IsSelf).FirstOrDefault();
            Country copySelf = copy.State.Where(c => c.IsSelf).FirstOrDefault();

            transformTemplate.Execute(copySelf);

            copySelf.State["population"].Should().Be(100);
            copySelf.State["metallicElements"].Should().Be(699);
            copySelf.State["timber"].Should().Be(1995);
            copySelf.State["metallicAlloys"].Should().Be(6);
            copySelf.State["housing"].Should().Be(1);
            copySelf.State["housingWaste"].Should().Be(1);

            initialSelf.State["population"].Should().Be(100);
            initialSelf.State["metallicElements"].Should().Be(700);
            initialSelf.State["timber"].Should().Be(2000);
            initialSelf.State["metallicAlloys"].Should().Be(9);
            initialSelf.State["housing"].Should().Be(0);
            initialSelf.State["housingWaste"].Should().Be(0);
        }
    }
}
