namespace TradeGame
{
    internal class Calculator : ICalculator
    {
        private const double gamma = 0.5;
        private const double x_0 = 0;
        private const double k = 1;
        private const double L = 1;
        private const int C = -3;

        public double CalculateExpectedUtility(IList<Action> schedule, Country initial_country, Country ending_country)
        {
            double discountedReward = CalculateDiscountedReward(schedule, initial_country, ending_country);
            double probabilityOfAcceptance = CalculateProbabilityOfAcceptance(schedule, initial_country, ending_country);

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
                double stateQuality = 0;

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

        public double CalculateUndiscountedReward(Country initial_country, Country ending_country)
        {
            // 0.0 is default value of double in C#
            if (initial_country.StateQuality == 0.0 || ending_country.StateQuality == 0.0)
            {
                CalculateStateQuality(new List<Country>() { initial_country, ending_country });
            }
            // round to 2 decimal places
            return Math.Round(ending_country.StateQuality - initial_country.StateQuality, 2);
        }

        public double CalculateDiscountedReward(IList<Action> schedule, Country initial_country, Country ending_country)
        {
            double undiscountedReward = CalculateUndiscountedReward(initial_country, ending_country);

            // round to 2 decimal places
            return Math.Round(Math.Pow(gamma, schedule.Count) * undiscountedReward, 2);
        }

        public double CalculateProbabilityOfAcceptance(IList<Action> schedule, Country initial_country, Country ending_country)
        {
            double x = CalculateDiscountedReward(schedule, initial_country, ending_country);
            double exponent = -k * (x - x_0);

            // round to 2 decimal places
            return Math.Round(L / (1 + Math.Pow(Math.E, exponent)), 2);
        }
    }
}
