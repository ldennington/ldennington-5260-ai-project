using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace TradeGame.Test
{
    [TestClass]
    public class ScheduleGeneratorTest
    {
        private TransformTemplate transformTemplate;
        private Country country;
        private Mock<ICalculator> calculatorMock = new Mock<ICalculator>();
        ScheduleGenerator generator;

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

            generator = new ScheduleGenerator(calculatorMock.Object);
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
        public void SetScale()
        {
            generator.SetScale(transformTemplate, country);
            transformTemplate.Scale.Should().Be(1);
        }

        [TestMethod]
        public void GenerateSuccessors()
        {

        }
    }
}
