namespace TradeGame
{
    internal class ExpectedUtilityCalculator
    {
        public void CalculateExpectedUtility(IList<ITemplate> schedule)
        {

        }

        public void CalculateStateQuality(IList<Country> world)
        {
            IDictionary<string, int> resourcesAndWeights = new Dictionary<string, int>();

            foreach(Country country in world)
            {
                foreach(string resource in country.State.Keys)
                {
                    switch(resource)
                    {
                        case "electronics":

                    }
                }
            }
        }
    }
}
