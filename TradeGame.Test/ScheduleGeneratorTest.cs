using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TradeGame.Test
{
    [TestClass]
    public class ScheduleGeneratorTest
    {
        private Mock<ICalculator> calculatorMock = new Mock<ICalculator>();
        private Mock<IReader> readerMock = new Mock<IReader>();
        private Mock<IWriter> writerMock = new Mock<IWriter>();

        private TransformTemplate transformTemplate;
        private Country country;
        private ScheduleGenerator generator;
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
                }
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

            generator = new ScheduleGenerator(readerMock.Object, writerMock.Object, calculatorMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // reset for setup of next test
            transformTemplate = new TransformTemplate();
            country = new Country();
        }
        
        [TestMethod]
        public void ExecuteTransform()
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
            generator.ExecuteTransform(transformTemplate, country);
            country.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void EnsureDeepCopyDoesNotModifyInitialState()
        {
            Node copy = initialNode.DeepCopy();

            Country initialSelf = initialNode.State.Where(c => c.IsSelf).FirstOrDefault();
            Country copySelf = copy.State.Where(c => c.IsSelf).FirstOrDefault();

            generator.ExecuteTransform(transformTemplate, copySelf);

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
