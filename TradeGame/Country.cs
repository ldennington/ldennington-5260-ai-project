using CsvHelper.Configuration.Attributes;

namespace TradeGame
{
    internal class Country
    {
        [Name("Country")]
        public string Name { get; set; }

        public IDictionary<string, int> State { get; set; }
    }
}
