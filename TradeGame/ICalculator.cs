namespace TradeGame
{
    public interface ICalculator
    {
        public double CalculateExpectedUtility(IList<IAction> schedule, Country initial_country, Country ending_country);

        public void CalculateStateQuality(Country country);

        public void CalculateStateQuality(IList<Country> world);

        public double CalculateUndiscountedReward(Country initial_country, Country ending_country);

        public double CalculateDiscountedReward(IList<IAction> schedule, Country initial_country, Country ending_country);

        public double CalculateProbabilityOfAcceptance(IList<IAction> schedule, Country initial_country, Country ending_country);
    }
}
