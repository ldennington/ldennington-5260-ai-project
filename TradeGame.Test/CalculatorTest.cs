using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TradeGame.Test
{
    [TestClass]
    public class CalculatorTest
    {
        private Country initial_country;
        private Country final_country;
        private IList<IAction> schedule;
        private IList<Country> twoCountryState;
        ICalculator calculator;

        [TestInitialize]
        public void Initialize()
        {
            // set up dictionary of resources for each test
            Global.Resources.Add("electronics", new Resource { Name = "electronics", Weight = 5 });
            Global.Resources.Add("metallicElements", new Resource { Name = "metallicElements", Weight = 4 });
            Global.Resources.Add("metallicAlloys", new Resource { Name = "metallicAlloys", Weight = 3 });
            Global.Resources.Add("housing", new Resource { Name = "housing", Weight = 2 });
            Global.Resources.Add("timber", new Resource { Name = "timber", Weight = 1 });

            initial_country = new Country()
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

            final_country = new Country()
            {
                Name = "Atlantis",
                State = new Dictionary<string, int>()
                {
                    { "population", 80 },
                    { "metallicElements", 900 },
                    { "timber", 600 },
                    { "metallicAlloys", 106 },
                    { "electronics", 380 },
                    { "housing", 170 }
                }
            };

            schedule = new List<IAction>()
            {
                new TransformTemplate()
                {
                    Name = "housing",
                    Country = "Atlantis",
                    Inputs = new Dictionary<string, int>()
                            {
                                { "population", 25 },
                                { "metallicElements", 5 },
                                { "timber", 25 },
                                { "metallicAlloys", 15 },
                            },
                    Outputs = new Dictionary<string, int>()
                            {
                                { "housing", 5 },
                                { "housingWaste", 5 },
                                { "population", 25 },
                            }
                },
                new TransformTemplate()
                {
                    Name = "electronics",
                    Country = "Atlantis",
                    Inputs = new Dictionary<string, int>()
                            {
                                { "population", 2 },
                                { "metallicElements", 6 },
                                { "metallicAlloys", 4 },
                            },
                    Outputs = new Dictionary<string, int>()
                            {
                                { "electronicsWaste", 1 },
                                { "electronics", 2 },
                                { "population", 2 },
                            }
                },
            };

            twoCountryState = new List<Country>()
            {
                new Country()
                {
                    Name = "Atlantis",
                    IsSelf = true,
                    State = new Dictionary<string, int>()
                    {
                        { "population", 150 },
                        { "metallicElements", 100 },
                        { "timber", 1100 },
                        { "metallicAlloys", 300 },
                        { "electronics", 506 },
                        { "housing", 200 }
                    }
                },
                new Country()
                {
                    Name = "Brobdingnag",
                    State = new Dictionary<string, int>()
                    {
                        { "population", 120 },
                        { "metallicElements", 950 },
                        { "timber", 600 },
                        { "metallicAlloys", 1000 },
                        { "electronics", 204 },
                        { "housing", 105 }
                    }
                },
            };

            calculator = new Calculator();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // reset for setup of next test
            Global.Resources = new Dictionary<string, Resource>();
            initial_country = new Country();
            final_country = new Country();
            schedule.Clear();
            twoCountryState.Clear();
        }

        [TestMethod]
        public void CalculateExpectedUtility()
        {
            double expectedUtility = 9.07;
            calculator.CalculateExpectedUtility(schedule, initial_country, final_country).Should().Be(expectedUtility);
        }

        [TestMethod]
        public void CalculateStateQuality()
        {
            double atlantisExpectedStateQuality = 35.53;
            double brobdingnagExpectedStateQuality = 71.92;

            calculator.CalculateStateQuality(twoCountryState);

            double atlantisStateQuality = twoCountryState.Where(c => c.Name.Equals("atlantis", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault().StateQuality;
            double brobdingnagStateQuality = twoCountryState.Where(c => c.Name.Equals("brobdingnag", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault().StateQuality;

            atlantisStateQuality.Should().Be(atlantisExpectedStateQuality);
            brobdingnagStateQuality.Should().Be(brobdingnagExpectedStateQuality);
        }

        [TestMethod]
        public void CalculateUndiscountedReward()
        {
            double expectedUndiscountedReward = 36.30;
            calculator.CalculateUndiscountedReward(initial_country, final_country).Should().Be(expectedUndiscountedReward);
        }

        [TestMethod]
        public void CalculateDiscountedReward()
        {
            double expectedDiscountedReward = 9.07;
            calculator.CalculateDiscountedReward(schedule, initial_country, final_country).Should().Be(expectedDiscountedReward);
        }

        [TestMethod]
        public void CalculateProbabilityOfParticipation()
        {
            double expectedProbability = 1.00;
            calculator.CalculateProbabilityOfAcceptance(schedule, initial_country, final_country).Should().Be(expectedProbability);
        }
    }
}
