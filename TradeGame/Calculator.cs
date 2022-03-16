namespace TradeGame
{
    internal class Calculator : ICalculator
    {
        private double gamma = 0.9; // 0 <= gamma < 1
        private double x_0 = -5; // 0 as starting point
        private double k = 1; // 1 as starting point
        private double L = 1;
        private int C = -2;

        public void CalculateExpectedUtility(Schedule schedule, IList<Country> worldInitialState, IList<Country> worldEndingState)
        {
            Country selfInitialState = worldInitialState.Where(c => c.IsSelf).FirstOrDefault();
            Country selfEndingState = worldEndingState.Where(c => c.IsSelf).FirstOrDefault();

            double discountedReward = CalculateDiscountedReward(schedule, selfInitialState, selfEndingState);
            double probabilityOfAcceptance = CalculateProbabilityOfAcceptance(schedule, worldInitialState, worldEndingState);

            schedule.Actions.Last().ExpectedUtility = Math.Round(probabilityOfAcceptance * discountedReward + (1-probabilityOfAcceptance) * C, 4);
        }

        /* State Quality Measure
         * Ecological footprint definition: amount of productive land a population uses
         * to sustain itself (including ridding itself of waste)
         * Ecological footprint equation (extremely simplified for our purposes):
         * 
         *              WeightedFood + WeightedHousing + WeightedElectronics + WeightedWastes
         *          -------------------------------------------------------------------------------
         *                                  WeightedAvailableland
         * 
         * A country should have a minimum of the following per person for quality of life:
         *      * 3 units of food
         *      * 0.5 units of housing
         *      * 1 unit of electronics
         * If a country does not meet this bar, it is penalized with 1 point added to its
         * ecological footprint
         * 
         * Once this bar is met, countries should try to minimize their ecological footprint
         * 
         * Definition and background on ecological footprint:
         *      https://www.footprintnetwork.org/content/documents/EF2006technotes2.pdf
         *      https://www.treehugger.com/what-is-ecological-footprint-4580244
         */
        public void CalculateStateQuality(Country country)
        {
            // note that in the current setup, population will never be 0 (since every country has
            // population inputs, population transfers are not permitted, and transforms do not
            // decrease population)
            double population = 0.0;
            double weightedFood = 0.0;
            double weightedHousing = 0.0;
            double weightedElectronics = 0.0;
            double weightedAvailableLand = 0.0;
            double weightedWaste = 0.0;
            double ecologicalFootprint = 0.0;

            foreach (Resource resource in Global.Resources.Values)
            {
                switch (resource.Name)
                {
                    case "Population":
                        population = country.State["Population"];
                        break;
                    case "Food":
                        weightedFood = WeightResource(country, resource.Name);
                        break;
                    case "Housing":
                        weightedHousing = WeightResource(country, resource.Name);
                        break;
                    case "Electronics":
                        weightedElectronics = WeightResource(country, resource.Name);
                        break;
                    case "Available Land":
                        weightedAvailableLand = WeightResource(country, resource.Name);
                        break;
                    case "Food Waste":
                    case "Farm Waste":
                    case "Housing Waste":
                    case "Electronics Waste":
                    case "Metallic Alloys Waste":
                        weightedWaste += WeightResource(country, resource.Name);
                        break;
                }
            }

            double foodPerPerson = country.State["Food"] / population;
            double housingPerPerson = country.State["Housing"] / population;
            double electronicsPerPerson = country.State["Electronics"] / population;

            if (foodPerPerson < 3 || housingPerPerson < 0.5 || electronicsPerPerson < 1.0)
            {
                ecologicalFootprint += 1.0;
            }

            ecologicalFootprint += (weightedFood + weightedHousing + weightedElectronics + weightedWaste) / weightedAvailableLand;

            // use the inverse to correctly reward for lower ecological footprints
            country.StateQuality = Math.Round(1/ecologicalFootprint, 4);
        }

        public double WeightResource(Country country, string resource)
        {
            return country.State[resource] *
                        Global.Resources.Where(r => r.Key.Equals(resource)).FirstOrDefault().Value.Weight;
        }

        public double CalculateUndiscountedReward(Country initialState, Country endingState)
        {
            // 0.0 is default value of a double in C#
            if (initialState.StateQuality == 0.0)
            {
                CalculateStateQuality(initialState);
            }

            if (endingState.StateQuality == 0.0)
            {
                CalculateStateQuality(endingState);
            }

            return Math.Round(endingState.StateQuality - initialState.StateQuality, 4);
        }

        public double CalculateDiscountedReward(Schedule schedule, Country initialState, Country endingState)
        {
            double undiscountedReward = CalculateUndiscountedReward(initialState, endingState);

            return Math.Round(Math.Pow(gamma, schedule.Actions.Count) * undiscountedReward, 4);
        }

        public double CalculateProbabilityOfAcceptance(Schedule schedule, IList<Country> worldInitialState, IList<Country> worldEndingState)
        {
            HashSet<string> participatingCountries = GetParticipatingCountries(schedule);
            List<double> countryProbabilitiesOfAcceptance = new List<double>();
            double overallProbabilityOfAcceptance = 1.0;

            foreach (string country in participatingCountries)
            {
                Country countryInitialState = worldInitialState.Where(c => c.Name.Equals(country, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                Country countryEndingState = worldEndingState.Where(c => c.Name.Equals(country, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                double discountedReward = CalculateDiscountedReward(schedule, countryInitialState, countryEndingState);
                double exponent = -k * (discountedReward - x_0);

                countryProbabilitiesOfAcceptance.Add(L / (1 + Math.Pow(Math.E, exponent)));
            }

            foreach (double probability in countryProbabilitiesOfAcceptance)
            {
                overallProbabilityOfAcceptance *= probability;
            }

            return Math.Round(overallProbabilityOfAcceptance, 4);
        }

        public HashSet<string> GetParticipatingCountries(Schedule schedule)
        {
            HashSet<string> participatingCountries = new HashSet<string>();

            foreach (Action action in schedule.Actions)
            {
                switch (action.GetType().Name)
                {
                    case "TransformTemplate":
                        TransformTemplate transformTemplate = (TransformTemplate)action;
                        participatingCountries.Add(transformTemplate.Country);
                        break;
                    case "TransferTemplate":
                        TransferTemplate transferTemplate = (TransferTemplate)action;
                        participatingCountries.Add(transferTemplate.TransferringCountry);
                        participatingCountries.Add(transferTemplate.ReceivingCountry);
                        break;
                }
            }

            return participatingCountries;
        }
    }
}
