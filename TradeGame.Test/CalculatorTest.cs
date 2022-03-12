using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
            Global.Resources.Add("Population", new Resource { Name = "Population", Weight = 0.0 });
            Global.Resources.Add("Metallic Elements", new Resource { Name = "Metallic Elements", Weight = 0.0 });
            Global.Resources.Add("Timber", new Resource { Name = "Timber", Weight = 0.0 });
            Global.Resources.Add("Metallic Alloys", new Resource { Name = "Metallic Alloys", Weight = 0.0 });
            Global.Resources.Add("Metallic Alloys Waste", new Resource { Name = "Metallic Alloys Waste", Weight = 0.3 });
            Global.Resources.Add("Electronics", new Resource { Name = "Electronics", Weight = 0.5 });
            Global.Resources.Add("Electronics Waste", new Resource { Name = "Electronics Waste", Weight = 0.3 });
            Global.Resources.Add("Housing", new Resource { Name = "Housing", Weight = 0.5 });
            Global.Resources.Add("Housing Waste", new Resource { Name = "Housing Waste", Weight = 0.3 });
            Global.Resources.Add("Available Land", new Resource { Name = "Available Land", Weight = 1.0 });
            Global.Resources.Add("Water", new Resource { Name = "Water", Weight = 0.0 });
            Global.Resources.Add("Farm", new Resource { Name = "Farm", Weight = 0.0 });
            Global.Resources.Add("Farm Waste", new Resource { Name = "Farm Waste", Weight = 0.3 });
            Global.Resources.Add("Food", new Resource { Name = "Food", Weight = 0.5 });
            Global.Resources.Add("Food Waste", new Resource { Name = "Food Waste", Weight = 0.3 });

            selfInitialState = new Country()
            {
                Name = "atlantis",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "Population", 100 },
                    { "Metallic Elements", 120 },
                    { "Timber", 50 },
                    { "Metallic Alloys", 100 },
                    { "Metallic Alloys Waste", 100 },
                    { "Electronics", 30 },
                    { "Electronics Waste", 30 },
                    { "Housing", 50 },
                    { "Housing Waste", 50 },
                    { "Available Land", 200 },
                    { "Water", 300 },
                    { "Farm", 10 },
                    { "Farm Waste", 10 },
                    { "Food", 300 },
                    { "Food Waste", 150 },
                }
            };

            selfEndingState = new Country()
            {
                Name = "atlantis",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "Population", 100 },
                    { "Metallic Elements", 80 },
                    { "Timber", 50 },
                    { "Metallic Alloys", 35 },
                    { "Metallic Alloys Waste", 365 },
                    { "Electronics", 100 },
                    { "Electronics Waste", 100 },
                    { "Housing", 50 },
                    { "Housing Waste", 50 },
                    { "Available Land", 350 },
                    { "Water", 480 },
                    { "Farm", 35 },
                    { "Farm Waste", 35 },
                    { "Food", 350 },
                    { "Food Waste", 100 },
                }
            };

            otherCountryInitialState = new Country()
            {
                Name = "carpania",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "Population", 25 },
                    { "Metallic Elements", 100 },
                    { "Timber", 300 },
                    { "Metallic Alloys", 350 },
                    { "Metallic Alloys Waste", 175 },
                    { "Electronics", 15 },
                    { "Electronics Waste", 8 },
                    { "Housing", 18 },
                    { "Housing Waste", 18 },
                    { "Available Land", 100 },
                    { "Water", 150 },
                    { "Farm", 3 },
                    { "Farm Waste", 3 },
                    { "Food", 65 },
                    { "Food Waste", 65 },
                }
            };

            otherCountryEndingState = new Country()
            {
                Name = "carpania",
                IsSelf = true,
                State = new Dictionary<string, int>()
                {
                    { "Population", 25 },
                    { "Metallic Elements", 88 },
                    { "Timber", 900 },
                    { "Metallic Alloys", 375 },
                    { "Metallic Alloys Waste", 300 },
                    { "Electronics", 75 },
                    { "Electronics Waste", 75 },
                    { "Housing", 60 },
                    { "Housing Waste", 60 },
                    { "Available Land", 480 },
                    { "Water", 80 },
                    { "Farm", 13 },
                    { "Farm Waste", 13 },
                    { "Food", 80 },
                    { "Food Waste", 80 },
                }
            };

            worldInitialState = new List<Country>() { selfInitialState, otherCountryInitialState };
            worldEndingState = new List<Country>() { selfEndingState, otherCountryEndingState };

            schedule = new Schedule()
            {
                Actions = new List<Action>()
                {
                    new TransformTemplate()
                    {
                        Name = "Housing",
                        Country = "atlantis",
                        Inputs = new Dictionary<string, int>()
                                {
                                    { "Population", 25 },
                                    { "Metallic Elements", 5 },
                                    { "Timber", 25 },
                                    { "Metallic Alloys", 15 },
                                },
                        Outputs = new Dictionary<string, int>()
                                {
                                    { "Housing", 5 },
                                    { "Housing Waste", 5 },
                                    { "Population", 25 },
                                }
                    },
                    new TransformTemplate()
                    {
                        Name = "Electronics",
                        Country = "atlantis",
                        Inputs = new Dictionary<string, int>()
                                {
                                    { "Population", 2 },
                                    { "Metallic Elements", 6 },
                                    { "Metallic Alloys", 4 },
                                },
                        Outputs = new Dictionary<string, int>()
                                {
                                    { "Electronics Waste", 1 },
                                    { "Electronics", 2 },
                                    { "Population", 2 },
                                }
                    },
                    new TransferTemplate()
                    {
                        TransferringCountry = "carpania",
                        ReceivingCountry = "atlantis",
                        Resource = "Food",
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
            schedule.Actions.Clear();
        }

        [TestMethod]
        public void CalculateExpectedUtility()
        {
            double expectedUtility = 0.2599;
            calculator.CalculateExpectedUtility(schedule, worldInitialState, worldEndingState);
            schedule.Actions.Last().ExpectedUtility.Should().Be(expectedUtility);
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
            double expectedDiscountedReward = 0.2770;
            calculator.CalculateDiscountedReward(schedule, selfInitialState, selfEndingState).Should().Be(expectedDiscountedReward);
        }

        [TestMethod]
        public void CalculateProbabilityOfParticipation()
        {
            double expectedProbability = 0.9925;
            calculator.CalculateProbabilityOfAcceptance(schedule, worldInitialState, worldEndingState).Should().Be(expectedProbability);
        }

        [TestMethod]
        public void WeightResource()
        {
            calculator.WeightResource(selfEndingState, "Electronics").Should().Be(50.0);
        }

        [TestMethod]
        public void GetParticipatingCountries()
        {
            calculator.GetParticipatingCountries(schedule).Should().BeEquivalentTo(new HashSet<string>() { "atlantis", "carpania" });
        }
    }
}
