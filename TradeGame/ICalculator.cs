namespace TradeGame
{
    public interface ICalculator
    {
         double CalculateExpectedUtility(IList<Action> schedule, Country initial_country, Country ending_country);

         void CalculateStateQuality(Country country);

         void CalculateStateQuality(IList<Country> world);

         double CalculateUndiscountedReward(Country initial_country, Country ending_country);

         double CalculateDiscountedReward(IList<Action> schedule, Country initial_country, Country ending_country);

         double CalculateProbabilityOfAcceptance(IList<Action> schedule, Country initial_country, Country ending_country);
    }
}
