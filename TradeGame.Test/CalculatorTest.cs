using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TradeGame.Test
{
    [TestClass]
    public class CalculatorTest
    {
        private Country selfInitialState;
        private Country selfEndingState;
        private Country otherCountryInitialState;
        private Country otherCountryEndingState;
        private Schedule schedule;
        ICalculator calculator;

        private IList<Country> worldInitialState;
        private IList<Country> worldEndingState;

        [TestInitialize]
        public void Initialize()
        {
            // set up dictionary of resources for each test
            Global.Resources.Add("population", new Resource { Name = "population", Weight = 0.0 });
            Global.Resources.Add("metallicElements", new Resource { Name = "metallicElements", Weight = 0.0 });
            Global.Resources.Add("timber", new Resource { Name = "timber", Weight = 0.0 });
            Global.Resources.Add("metallicAlloys", new Resource { Name = "metallicAlloys", Weight = 0.0 });
            Global.Resources.Add("metallicAlloysWaste", new Resource { Name = "metallicAlloysWaste", Weight = 0.3 });
            Global.Resources.Add("electronics", new Resource { Name = "electronics", Weight = 0.5 });
            Global.Resources.Add("electronicsWaste", new Resource { Name = "electronicsWaste", Weight = 0.3 });
            Global.Resources.Add("housing", new Resource { Name = "housing", Weight = 0.5 });
            Global.Resources.Add("housingWaste", new Resource { Name = "housingWaste", Weight = 0.3 });
            Global.Resources.Add("availableLand", new Resource { Name = "availableLand", Weight = 1.0 });
            Global.Resources.Add("water", new Resource { Name = "water", Weight = 0.0 });
            Global.Resources.Add("farm", new Resource { Name = "farm", Weight = 0.0 });
            Global.Resources.Add("farmWaste", new Resource { Name = "farmWaste", Weight = 0.3 });
            Global.Resources.Add("food", new Resource { Name = "food", Weight = 0.5 });
            Global.Resources.Add("foodWaste", new Resource { Name = "foodWaste", Weight = 0.3 });

            selfInitialState = new Country()
            {
                Name = "atlantis",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "population", 100 },
                    { "metallicElements", 120 },
                    { "timber", 50 },
                    { "metallicAlloys", 100 },
                    { "metallicAlloysWaste", 100 },
                    { "electronics", 30 },
                    { "electronicsWaste", 30 },
                    { "housing", 50 },
                    { "housingWaste", 50 },
                    { "availableLand", 200 },
                    { "water", 300 },
                    { "farm", 10 },
                    { "farmWaste", 10 },
                    { "food", 300 },
                    { "foodWaste", 150 },
                }
            };

            selfEndingState = new Country()
            {
                Name = "atlantis",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "population", 100 },
                    { "metallicElements", 80 },
                    { "timber", 50 },
                    { "metallicAlloys", 35 },
                    { "metallicAlloysWaste", 365 },
                    { "electronics", 100 },
                    { "electronicsWaste", 100 },
                    { "housing", 50 },
                    { "housingWaste", 50 },
                    { "availableLand", 350 },
                    { "water", 480 },
                    { "farm", 35 },
                    { "farmWaste", 35 },
                    { "food", 350 },
                    { "foodWaste", 100 },
                }
            };

            otherCountryInitialState = new Country()
            {
                Name = "carpania",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "population", 25 },
                    { "metallicElements", 100 },
                    { "timber", 300 },
                    { "metallicAlloys", 350 },
                    { "metallicAlloysWaste", 175 },
                    { "electronics", 15 },
                    { "electronicsWaste", 8 },
                    { "housing", 18 },
                    { "housingWaste", 18 },
                    { "availableLand", 100 },
                    { "water", 150 },
                    { "farm", 3 },
                    { "farmWaste", 3 },
                    { "food", 65 },
                    { "foodWaste", 65 },
                }
            };

            otherCountryEndingState = new Country()
            {
                Name = "carpania",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "population", 25 },
                    { "metallicElements", 88 },
                    { "timber", 900 },
                    { "metallicAlloys", 375 },
                    { "metallicAlloysWaste", 300 },
                    { "electronics", 75 },
                    { "electronicsWaste", 75 },
                    { "housing", 60 },
                    { "housingWaste", 60 },
                    { "availableLand", 480 },
                    { "water", 80 },
                    { "farm", 13 },
                    { "farmWaste", 13 },
                    { "food", 80 },
                    { "foodWaste", 80 },
                }
            };

            worldInitialState = new List<Country>() { selfInitialState, otherCountryInitialState };
            worldEndingState = new List<Country>() { selfEndingState, otherCountryEndingState };

            schedule = new Schedule()
            {
                Steps = new List<Action>()
                {
                    new TransformTemplate()
                    {
                        Name = "housing",
                        Country = "atlantis",
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
                        Country = "atlantis",
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
                    new TransferTemplate()
                    {
                        TransferringCountry = "carpania",
                        ReceivingCountry = "atlantis",
                        Resource = "food",
                        Amount = 65
                    }
                }
            };

            calculator = new Calculator();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // reset for setup of next test
            Global.Resources = new Dictionary<string, Resource>();
            selfInitialState = new Country();
            selfEndingState = new Country();
            schedule.Steps.Clear();
        }

        [TestMethod]
        public void CalculateExpectedUtility()
        {
            double expectedUtility = -1.9742;
            calculator.CalculateExpectedUtility(schedule, worldInitialState, worldEndingState).Should().Be(expectedUtility);
        }

        [TestMethod]
        public void CalculateStateQuality()
        {
            double expected = 0.7865;
            calculator.CalculateStateQuality(selfEndingState);
            selfEndingState.StateQuality.Should().Be(expected);
        }

        [TestMethod]
        public void CalculateUndiscountedReward()
        {
            double expectedUndiscountedReward = 0.38;
            calculator.CalculateUndiscountedReward(selfInitialState, selfEndingState).Should().Be(expectedUndiscountedReward);
        }

        [TestMethod]
        public void CalculateDiscountedReward()
        {
            double expectedDiscountedReward = 0.1303;
            calculator.CalculateDiscountedReward(schedule, selfInitialState, selfEndingState).Should().Be(expectedDiscountedReward);
        }

        [TestMethod]
        public void CalculateProbabilityOfParticipation()
        {
            double expectedProbability = 0.3277;
            calculator.CalculateProbabilityOfAcceptance(schedule, worldInitialState, worldEndingState).Should().Be(expectedProbability);
        }

        [TestMethod]
        public void WeightResource()
        {
            calculator.WeightResource(selfEndingState, "electronics").Should().Be(50.0);
        }

        [TestMethod]
        public void GetParticipatingCountries()
        {
            calculator.GetParticipatingCountries(schedule).Should().BeEquivalentTo(new HashSet<string>() { "atlantis", "carpania" });
        }
    }
}
