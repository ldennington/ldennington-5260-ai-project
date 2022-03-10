namespace TradeGame
{
    public interface ICalculator
    {
        double CalculateExpectedUtility(Schedule schedule, IList<Country> worldInitialState, IList<Country> worldEndingState);

        void CalculateStateQuality(Country country);

        double WeightResource(Country country, string resource);

        double CalculateUndiscountedReward(Country initialCountry, Country endingCountry);

        double CalculateDiscountedReward(Schedule schedule, Country initialCountry, Country endingCountry);

        double CalculateProbabilityOfAcceptance(Schedule schedule, IList<Country> worldInitialState, IList<Country> worldEndingState);

        HashSet<string> GetParticipatingCountries(Schedule schedule);
    }
}
