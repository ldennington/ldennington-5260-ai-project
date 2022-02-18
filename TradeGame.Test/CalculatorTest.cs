using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TradeGame.Test
{
    [TestClass]
    public class CalculatorTest
    {
        [TestInitialize]
        public void Initialize()
        {
            // set up dictionary of resources for each test
            Global.Resources.Add("electronics", new Resource { Name = "electronics", Weight = 5 });
            Global.Resources.Add("metallicElements", new Resource { Name = "metallicElements", Weight = 4 });
            Global.Resources.Add("metallicAlloys", new Resource { Name = "metallicAlloys", Weight = 3 });
            Global.Resources.Add("housing", new Resource { Name = "housing", Weight = 2 });
            Global.Resources.Add("timber", new Resource { Name = "timber", Weight = 1 });
        }

        [TestCleanup]
        public void Cleanup()
        {
            // remove items from resource dictionary for setup of next test
            Global.Resources = new Dictionary<string, Resource>();
        }

        [TestMethod]
        public void CalculateExpectedUtility()
        {
            double expectedProbability = 9.07;

            Calculator calculator = new Calculator();
            calculator.CalculateExpectedUtility(TestData.SCHEDULE, TestData.INITIAL_COUNTRY, TestData.FINAL_COUNTRY).Should().Be(expectedProbability);
        }

        [TestMethod]
        public void CalculateStateQuality()
        {
            double atlantisExpectedStateQuality = 35.53;
            double brobdingnagExpectedStateQuality = 71.92;

            Calculator calculator = new Calculator();
            calculator.CalculateStateQuality(TestData.TWO_COUNTRY_STATE);

            double atlantisStateQuality = TestData.TWO_COUNTRY_STATE.Where(c => c.Name.Equals("atlantis", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault().StateQuality;
            double brobdingnagStateQuality = TestData.TWO_COUNTRY_STATE.Where(c => c.Name.Equals("brobdingnag", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault().StateQuality;

            atlantisStateQuality.Should().Be(atlantisExpectedStateQuality);
            brobdingnagStateQuality.Should().Be(brobdingnagExpectedStateQuality);
        }

        [TestMethod]
        public void CalculateUndiscountedReward()
        {
            double expectedUndiscountedReward = 36.30;

            Calculator calculator = new Calculator();
            calculator.CalculateUndiscountedReward(TestData.INITIAL_COUNTRY, TestData.FINAL_COUNTRY).Should().Be(expectedUndiscountedReward);
        }

        [TestMethod]
        public void CalculateDiscountedReward()
        {
            double expectedDiscountedReward = 9.07;

            Calculator calculator = new Calculator();
            calculator.CalculateDiscountedReward(TestData.SCHEDULE, TestData.INITIAL_COUNTRY, TestData.FINAL_COUNTRY).Should().Be(expectedDiscountedReward);
        }

        [TestMethod]
        public void CalculateProbabilityOfParticipation()
        {
            double expectedProbability = 1.00;

            Calculator calculator = new Calculator();
            calculator.CalculateProbabilityOfAcceptance(TestData.SCHEDULE, TestData.INITIAL_COUNTRY, TestData.FINAL_COUNTRY).Should().Be(expectedProbability);
        }
    }
}
