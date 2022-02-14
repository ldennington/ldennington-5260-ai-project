using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TradeGame.Test
{
    [TestClass]
    public class GenerateSuccessorsTest
    {
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
            TestData.TRANSFORM_TEMPLATE.Scale = 1;
            ScheduleGenerator generator = new ScheduleGenerator();
            generator.ExecuteTransform(TestData.TRANSFORM_TEMPLATE, TestData.COUNTRY);
            TestData.COUNTRY.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void SetScale()
        {
            ScheduleGenerator generator = new ScheduleGenerator();
            generator.SetScale(TestData.TRANSFORM_TEMPLATE, TestData.COUNTRY);
            TestData.TRANSFORM_TEMPLATE.Scale.Should().Be(1);
        }

        [TestMethod]
        public void GenerateSuccessors()
        {

        }
    }
}
