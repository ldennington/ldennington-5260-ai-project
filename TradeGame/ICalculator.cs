namespace TradeGame
{
    public interface ICalculator
    {
        void CalculateExpectedUtility(Schedule schedule, IList<Country> worldInitialState, IList<Country> worldEndingState, bool isFinal, bool testModel);

        void CalculateStateQuality(Country country);

        double WeightResource(Country country, string resource);

        double CalculateUndiscountedReward(Country initialCountry, Country endingCountry);

        double CalculateDiscountedReward(Schedule schedule, Country initialCountry, Country endingCountry);

        double CalculateProbabilityOfAcceptance(Schedule schedule, IList<Country> worldInitialState, IList<Country> worldEndingState);

        HashSet<string> GetParticipatingCountries(Schedule schedule);

        bool ShouldCalculate(Schedule schedule, out double predicted);
    }
}
