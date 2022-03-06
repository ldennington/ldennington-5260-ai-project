namespace TradeGame
{
    internal class Calculator : ICalculator
    {
        private const double gamma = 0.5;
        private const double x_0 = 0;
        private const double k = 1;
        private const double L = 1;
        private const int C = -3;

        public double CalculateExpectedUtility(IList<Action> schedule, Country initialState, Country endingState)
        {
            double discountedReward = CalculateDiscountedReward(schedule, initialState, endingState);
            double probabilityOfAcceptance = CalculateProbabilityOfAcceptance(schedule, initialState, endingState);

            // round to 2 decimal places
            return Math.Round(probabilityOfAcceptance * discountedReward + (1-probabilityOfAcceptance) * C, 2);
        }

        public void CalculateStateQuality(Country country)
        {
            CalculateStateQuality(new List<Country> { country });
        }

        public void CalculateStateQuality(IList<Country> world)
        {
            foreach(Country country in world)
            {
                double stateQuality = 0.00;

                // without population we can't normalize, so state quality is 0
                if (country.State["population"] == 0)
                {
                    country.StateQuality = stateQuality;
                    return;
                }

                // we don't count population in our score since we use it to normalize
                foreach (string item in country.State.Keys
                    .Where(k => !k.Equals("population", StringComparison.OrdinalIgnoreCase)))
                {
                    // find the resource in the Global Dictionary
                    Resource resource = Global.Resources.Where(r => r.Key
                        .Equals(item, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value;
                    // resource amount * weight / population
                    stateQuality += country.State[item] * resource.Weight / country.State["population"];
                }

                // round to 2 decimal places
                country.StateQuality = Math.Round(stateQuality, 2);
            }
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

            // round to 2 decimal places
            return Math.Round(endingState.StateQuality - initialState.StateQuality, 2);
        }

        public double CalculateDiscountedReward(IList<Action> schedule, Country initialState, Country endingState)
        {
            double undiscountedReward = CalculateUndiscountedReward(initialState, endingState);

            // round to 2 decimal places
            return Math.Round(Math.Pow(gamma, schedule.Count) * undiscountedReward, 2);
        }

        public double CalculateProbabilityOfAcceptance(IList<Action> schedule, Country initialState, Country endingState)
        {
            double x = CalculateDiscountedReward(schedule, initialState, endingState);
            double exponent = -k * (x - x_0);

            // round to 2 decimal places
            return Math.Round(L / (1 + Math.Pow(Math.E, exponent)), 2);
        }
    }
}
